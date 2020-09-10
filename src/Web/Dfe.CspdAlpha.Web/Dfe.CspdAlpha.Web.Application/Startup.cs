using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Application.Services;
using Dfe.CspdAlpha.Web.Application.Middleware;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.CspdAlpha.Web.Infrastructure.Interfaces;
using Dfe.CspdAlpha.Web.Infrastructure.CosmosDb.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.PowerPlatform.Cds.Client;
using Microsoft.Xrm.Sdk;
using Sustainsys.Saml2;
using Sustainsys.Saml2.AspNetCore2;
using Sustainsys.Saml2.Metadata;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Infrastructure.CosmosDb.DTOs;
using Dfe.CspdAlpha.Web.Infrastructure.CosmosDb.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using AppInterface = Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using DomainInterface = Dfe.CspdAlpha.Web.Domain.Interfaces;
using EnvironmentName = Microsoft.AspNetCore.Hosting.EnvironmentName;
using Dfe.CspdAlpha.Web.Shared.Config;
using Dfe.CspdAlpha.Web.Infrastructure.SharePoint;

namespace Dfe.CspdAlpha.Web.Application
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(config =>
            {
                // Action methods have to explicitly opt out to allow anonymous access
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            // This disables the CSRF token in order to facilitate easier QA for the time being
            if (_env.IsStaging())
            {
                services.AddMvc().AddRazorPagesOptions(o =>
                {
                    o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
                }).InitializeTagHelper<FormTagHelper>((helper, context) => helper.Antiforgery = false);
            }

            // configure SAML authentication
            var samlAuthOptions = Configuration.GetSection("SamlAuth").Get<SamlAuthOptions>();
            var authenticationBuilder = services.AddAuthentication(o =>
            {
                o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = Saml2Defaults.Scheme;
            })
            .AddCookie()
            .AddSaml2(options =>
            {
                options.SPOptions.EntityId = new EntityId(samlAuthOptions.SpEntityId);
                options.IdentityProviders.Add(
                    new IdentityProvider(
                        new EntityId($"{samlAuthOptions.IdpEntityId}/Metadata"), options.SPOptions)
                    {
                        LoadMetadata = true
                    });
            });

            if (_env.IsStaging())
            {
                services.Configure<BasicAuthOptions>(Configuration.GetSection("BasicAuth"));
            }

            // Dynamics 365 configuration
            var dynamicsConnString = Configuration.GetConnectionString("DynamicsCds");
            var cdsClient = new CdsServiceClient(dynamicsConnString);

            services.AddTransient<IOrganizationService, CdsServiceClient>(sp => cdsClient.Clone());
            services.Configure<DynamicsOptions>(Configuration.GetSection("Dynamics"));

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddSession(options =>
            { 
                options.Cookie.IsEssential = true;
            });

            services.Configure<SharePointOptions>(Configuration.GetSection("SharePoint"));


            services.AddApplicationInsightsTelemetry();

            var cosmosDbOptions = Configuration.GetSection("CosmosDb").Get<CosmosDbOptions>();
            var client = new CosmosClient(cosmosDbOptions.Account, cosmosDbOptions.Key);

            services.AddSingleton(IntialisePupilService(client, cosmosDbOptions.Database, cosmosDbOptions.PupilsCollection).GetAwaiter().GetResult());
            services.AddSingleton<IPupilService, PupilService>();
            services.AddSingleton(IntialiseEstablishmentService(client, cosmosDbOptions.Database, cosmosDbOptions.EstablishmentsCollection).GetAwaiter().GetResult());
            services.AddSingleton<IEstablishmentService, EstablishmentService>();
            services.AddSingleton<DomainInterface.IAmendmentService, CrmAmendmentService>();
            services.AddSingleton<IConfirmationService, CrmConfirmationService>();
            services.AddSingleton<IFileUploadService, SharePointFileUploadService>();
            services.AddSingleton<ISchoolService, SchoolService>();
            services.AddSingleton<AppInterface.IAmendmentService, AmendmentService>();
        }

        private static async Task<IReadRepository<EstablishmentsDTO>> IntialiseEstablishmentService(CosmosClient client, string database, string collection)
        {
            return new EstablishmentRepository(client, database, collection);
        }
        private static async Task<IReadRepository<PupilDTO>> IntialisePupilService(CosmosClient client, string database, string collection)
        {
            return new PupilRepository(client, database, collection);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                var provider = new FileExtensionContentTypeProvider();

                // Add .scss mapping
                provider.Mappings[".scss"] = "text/css";
                app.UseStaticFiles(new StaticFileOptions()
                {
                    ContentTypeProvider = provider
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

                app.UseStaticFiles();
            }

            app.UseHttpsRedirection();

            // staging = all hosted non-production environments
            if (env.IsStaging())
            {
                app.UseBasicAuth();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseSession();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "school",
                    pattern: "school/{urn}/{controller}/{action}/{id?}",
                    defaults: new {controller = "School", action = "Index"});

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

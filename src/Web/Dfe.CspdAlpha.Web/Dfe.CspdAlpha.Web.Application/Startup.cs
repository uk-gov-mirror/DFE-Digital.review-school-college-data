using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Application.Services;
using Dfe.CspdAlpha.Web.Application.Middleware;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.CspdAlpha.Web.Infrastructure.Interfaces;
using Dfe.CspdAlpha.Web.Infrastructure.SharePoint;
using Dfe.CspdAlpha.Web.Shared.Config;
using Dfe.Rscd.Web.ApiClient;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Microsoft.PowerPlatform.Cds.Client;
using Microsoft.Xrm.Sdk;
using Sustainsys.Saml2;
using Sustainsys.Saml2.AspNetCore2;
using Sustainsys.Saml2.Metadata;
using System;
using System.Net.Http.Headers;
using System.Text;
using AppInterface = Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using DomainInterface = Dfe.CspdAlpha.Web.Domain.Interfaces;
using FluentValidation.AspNetCore;
using Dfe.CspdAlpha.Web.Application.Validators.Pupil;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Dfe.CspdAlpha.Web.Application.TagHelpers;

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
            })
            .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<AddPupilDetailsViewModelValidator>();
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                });

            // Adds feature management for Azure App Configuration
            services.AddFeatureManagement();

            // This disables the CSRF token in order to facilitate easier QA for the time being
            if (_env.IsStaging())
            {
                services.AddMvc()
                    .AddRazorPagesOptions(o =>
                    {
                        o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
                    })
                    .InitializeTagHelper<FormTagHelper>((helper, context) => helper.Antiforgery = false);
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

            // Session config
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

            // Application insights config
            services.AddApplicationInsightsTelemetry();

            services.AddSingleton<DomainInterface.IAmendmentService, CrmAmendmentService>();
            services.AddSingleton<IConfirmationService, CrmConfirmationService>();
            services.AddSingleton<IFileUploadService, SharePointFileUploadService>();
            services.AddSingleton<ISchoolService, SchoolService>();
            services.AddSingleton<AppInterface.IEstablishmentService, EstablishmentService>();
            services.AddSingleton<AppInterface.IAmendmentService, AmendmentService>();
            services.AddTransient<IHtmlGenerator, HtmlGenerator>();

            var apiOptions = Configuration.GetSection("Api").Get<ApiOptions>();
            services.AddHttpClient<IClient, Client>(client =>
            {
                client.BaseAddress = new Uri(apiOptions.URL);
                if (!string.IsNullOrWhiteSpace(apiOptions.UserName) && !string.IsNullOrWhiteSpace(apiOptions.Password))
                {
                    var encodedCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{apiOptions.UserName}:{apiOptions.Password}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);
                }
            });
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
                    name: "checking-phase",
                    pattern: "{phase}/{urn}/{controller}/{action}/{id?}",
                    defaults: new {controller = "TaskList", action = "Index"});

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseAzureAppConfiguration();
        }
    }
}

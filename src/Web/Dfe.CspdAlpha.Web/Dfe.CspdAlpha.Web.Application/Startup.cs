using System;
using System.Net.Http.Headers;
using System.Text;
using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Application;
using Dfe.Rscd.Web.Application.Application.Cache.Redis;
using Dfe.Rscd.Web.Application.Application.Interfaces;
using Dfe.Rscd.Web.Application.Application.Services;
using Dfe.Rscd.Web.Application.Middleware;
using Dfe.Rscd.Web.Application.Security;
using Dfe.Rscd.Web.Application.TagHelpers;
using Dfe.Rscd.Web.Application.Validators.RemovePupil;
using Dfe.Rscd.Web.Infrastructure.Interfaces;
using Dfe.Rscd.Web.Infrastructure.SharePoint;
using Dfe.Rscd.Web.Shared.Config;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Newtonsoft.Json;
using Serilog;

namespace Dfe.Rscd.Web.Application
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
                    fv.RegisterValidatorsFromAssemblyContaining<SearchPupilsViewModelValidator>();
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                }).AddNewtonsoftJson(o => { o.SerializerSettings.TypeNameHandling = TypeNameHandling.All; });

            // Adds feature management for Azure App Configuration
            services.AddFeatureManagement();
            services.AddAzureAppConfiguration();
            services.AddHttpContextAccessor();
            services.AddResponseCaching();
            services.AddResponseCompression();

            // This disables the CSRF token in order to facilitate easier QA for the time being
            if (_env.IsDevelopment() || _env.IsStaging())
            {
                services.AddMvc()
                    .AddRazorPagesOptions(o =>
                    {
                        o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
                    })
                    .InitializeTagHelper<FormTagHelper>((helper, context) => helper.Antiforgery = false);
            }

            //services.AddControllers().AddJsonOptions()

            var dfeSignInSettings = Configuration.GetSection(nameof(DfeSignInSettings));
            services.Configure<DfeSignInSettings>(dfeSignInSettings);
            services.AddDfeSignIn(dfeSignInSettings.Get<DfeSignInSettings>());

            if (_env.IsDevelopment() || _env.IsStaging())
            {
                services.Configure<BasicAuthOptions>(Configuration.GetSection("BasicAuth"));
            }

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

            // Caching config
            Program.RedisConnectionString = Configuration.GetConnectionString("RedisCache");
            services.AddSingleton<IRedisCache, RedisCache>();
                        
            if (Configuration.GetValue<bool>("EnableCache"))
            {
                services.AddSingleton<ISchoolService, CachedSchoolService>();
                services.AddSingleton<IEstablishmentService, CachedEstablishmentService>();
                services.AddSingleton<IPupilService, CachedPupilService>();
                services.AddSingleton<IAmendmentService, CachedAmendmentService>();
            }
            else
            {
                services.AddSingleton<ISchoolService, SchoolService>();
                services.AddSingleton<IEstablishmentService, EstablishmentService>();
                services.AddSingleton<IPupilService, PupilService>();
                services.AddSingleton<IAmendmentService, AmendmentService>();
            }            

            // Application insights config
            services.AddApplicationInsightsTelemetry();

            services.AddSingleton<IFileUploadService, SharePointFileUploadService>();
            services.AddSingleton<IEvidenceService, EvidenceService>();
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
            if (env.IsEnvironment(Program.LOCAL_ENVIRONMENT) || env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            if (env.IsEnvironment(Program.LOCAL_ENVIRONMENT))
            {
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
                app.UseStaticFiles();

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            Context.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());

            app.UseHttpsRedirection();

            // Uncomment in future if we want to push request logs to Serilog sinks.
            //app.UseSerilogRequestLogging();

            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseBasicAuth();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseSession();

            app.UseResponseCaching();
            app.UseResponseCompression();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "checking-phase",
                    pattern: "{phase}/{urn:int}/{controller}/{action}/{id?}",
                    defaults: new {controller = "TaskList", action = "Index"});

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseAzureAppConfiguration();
        }
    }
}

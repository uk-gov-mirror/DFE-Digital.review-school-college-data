using System;
using System.Net.Http.Headers;
using System.Text;
using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Application;
using Dfe.Rscd.Web.Application.Application.Interfaces;
using Dfe.Rscd.Web.Application.Application.Services;
using Dfe.Rscd.Web.Application.Middleware;
using Dfe.Rscd.Web.Application.TagHelpers;
using Dfe.Rscd.Web.Application.Validators.AddPupil;
using Dfe.Rscd.Web.Infrastructure.Interfaces;
using Dfe.Rscd.Web.Infrastructure.SharePoint;
using Dfe.Rscd.Web.Shared.Config;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
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
                    fv.RegisterValidatorsFromAssemblyContaining<AddPupilViewModelValidator>();
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

            // configure OpenID Connect authentication
            var oidcAuthOptions = Configuration.GetSection("OidcAuth").Get<OidcAuthOptions>();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login/";
            })
            .AddOpenIdConnect(options =>
            {
                options.MetadataAddress = oidcAuthOptions.MetadataUrl;
                options.CallbackPath = "/auth/cb";
                options.SignedOutCallbackPath = "/signout/complete";
                options.ClientId = oidcAuthOptions.ClientId;
                options.ClientSecret = oidcAuthOptions.ClientSecret;                
                options.ResponseType = "code";
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("email");
                options.Scope.Add("profile");
                options.Scope.Add("organisation");
                //options.Scope.Add("organisationid");
                //options.Scope.Add("offline_access");
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = false;
            });

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

            // Application insights config
            services.AddApplicationInsightsTelemetry();

            services.AddSingleton<IFileUploadService, SharePointFileUploadService>();
            services.AddSingleton<ISchoolService, SchoolService>();
            services.AddSingleton<IEstablishmentService, EstablishmentService>();
            services.AddSingleton<IPupilService, PupilService>();
            services.AddSingleton<IEvidenceService, EvidenceService>();
            services.AddSingleton<IAmendmentService, AmendmentService>();
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

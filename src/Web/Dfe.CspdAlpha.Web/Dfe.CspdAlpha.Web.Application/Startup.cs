using Dfe.CspdAlpha.Web.Application.Config;
using Dfe.CspdAlpha.Web.Application.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sustainsys.Saml2;
using Sustainsys.Saml2.AspNetCore2;
using Sustainsys.Saml2.Metadata;

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

            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.None;
            });

            services.AddApplicationInsightsTelemetry();
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
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

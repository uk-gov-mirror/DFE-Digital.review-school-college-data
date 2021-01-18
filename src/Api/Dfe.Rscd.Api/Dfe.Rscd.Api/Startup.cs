using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Services;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Config;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Services;
using Dfe.Rscd.Api.Middleware.BasicAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.PowerPlatform.Cds.Client;
using Microsoft.Xrm.Sdk;
using System;
using System.Text.Json.Serialization;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Interfaces;
using Newtonsoft.Json;
using Microsoft.FeatureManagement;

namespace Dfe.Rscd.Api
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
            services.AddControllers()
                .AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .AddNewtonsoftJson(o => { o.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto; });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Review school and college data API - V1",
                        Version = "V1",
                        Description =
                            "Provides a restful API for intergating with the RSCD Dynamics CRM Common Data Service and CosmosDB data sources",
                        License = new OpenApiLicense
                        {
                            Name = "MIT License",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        }
                    });
                c.EnableAnnotations();
            });

            // Adds feature management for Azure App Configuration
            services.AddFeatureManagement();
            services.AddAzureAppConfiguration();

            // Dynamics 365 configuration
            var dynamicsConnString = Configuration.GetConnectionString("DynamicsCds");
            var cdsClient = new CdsServiceClient(dynamicsConnString);
            services.AddTransient<IOrganizationService, CdsServiceClient>(sp => cdsClient.Clone());
            services.Configure<DynamicsOptions>(Configuration.GetSection("Dynamics"));


            if (_env.IsStaging())
            {
                services.Configure<BasicAuthOptions>(Configuration.GetSection("BasicAuth"));
            }
            services.Configure<CosmosDbOptions>(Configuration.GetSection("CosmosDb"));
            services.AddSingleton<IEstablishmentService, EstablishmentService>();
            services.AddSingleton<IPupilService, PupilService>();
            services.AddSingleton<IAmendmentService, CrmAmendmentService>();
            services.AddSingleton<IAmendmentBuilder, AmendmentBuilder>();
            services.AddSingleton<IOutcomeService, OutcomeService>();
            services.AddSingleton<IConfirmationService, CrmConfirmationService>();

            if (!_env.IsEnvironment(Program.LOCAL_ENVIRONMENT))
            {
                services.Configure<BasicAuthOptions>(Configuration.GetSection("BasicAuth"));
            }
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // This needs to come before swagger
            if (!_env.IsEnvironment(Program.LOCAL_ENVIRONMENT))
            {
                app.UseBasicAuth();
            };

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Review school and college data API V1");
                c.RoutePrefix = string.Empty;
            }
            );

            if (_env.IsEnvironment(Program.LOCAL_ENVIRONMENT) || env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAzureAppConfiguration();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

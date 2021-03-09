using System;
using System.Text.Json.Serialization;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Entities.Amendments;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Config;
using Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs;
using Dfe.Rscd.Api.Infrastructure.SqlServer.Repositories;
using Dfe.Rscd.Api.Middleware.BasicAuth;
using Dfe.Rscd.Api.Services;
using Dfe.Rscd.Api.Services.Rules;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using Microsoft.PowerPlatform.Cds.Client;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;

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
                .AddJsonOptions(x => { x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); })
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
                c.UseAllOfToExtendReferenceSchemas();
            });

            // Adds feature management for Azure App Configuration
            services.AddFeatureManagement();
            services.AddAzureAppConfiguration();

            if (!_env.IsProduction())
            {
                // obtain CRM request messages in non-prod environments
                services.AddSingleton<ITelemetryInitializer, CrmTelemetryInitializer>();
            }

            services.AddApplicationInsightsTelemetry();

            var referenceDataConnectionString = Configuration.GetConnectionString("ReferenceData");

            services.AddDbContext<SqlDataRepositoryContext>(options =>
                options.UseSqlServer(
                    referenceDataConnectionString,
                    providerOptions => providerOptions.EnableRetryOnFailure()));

            services.AddScoped<IDataRepository, DataRepository>();
            services.AddScoped<IDataService, DataService>();

            // Dynamics 365 configuration
            var dynamicsConnString = Configuration.GetConnectionString("DynamicsCds");

            var cdsClient = new CdsServiceClient(dynamicsConnString);
            services.AddTransient<IOrganizationService, CdsServiceClient>(sp => cdsClient.Clone());

            // Microsoft Extensions Configuration (services.Configure) is still in Alpha.
            // This affects EntityFramework Core in Infra Assembly
            // So we do it the old fashioned way for now
            var dynamicsOptions = Configuration.GetSection("Dynamics").Get<DynamicsOptions>();
            services.AddSingleton(dynamicsOptions);

            services.AddSingleton<IAllocationYearConfig>(new AllocationYearConfig
                {Value = Configuration["AllocationYear"], CensusDate = Configuration["AnnualCensusDate"]});

            if (!_env.IsEnvironment(Program.LOCAL_ENVIRONMENT))
                services.Configure<BasicAuthOptions>(Configuration.GetSection("BasicAuth"));
            ;

            // Microsoft Extensions Configuration (services.Configure) is still in Alpha.
            // This affects EntityFramework Core in Infra Assembly
            // So we do it the old fashioned way for now
            var cosmosDbOptions = Configuration.GetSection("CosmosDb").Get<CosmosDbOptions>();
            services.AddSingleton(cosmosDbOptions);

            services.AddSingleton<IDocumentRepository, CosmosDocumentRepository>();

            services.AddScoped<IRule, RemovePupilAdmittedFromAbroadRule>();
            services.AddScoped<IRule, RemovePupilAdmittedFollowingPermanentExclusion>();
            services.AddScoped<IRule, RemovePupilDeceased>();
            services.AddScoped<IRule, RemovePupilPermanentlyLeftEngland>();
            services.AddScoped<IRule, RemovePupilOtherTerminalLongIllnessRule>();
            services.AddScoped<IRule, RemovePupilOtherPoliceInvolvementBailRule>();
            services.AddScoped<IRule, RemovePupilOtherSafeguardingFapRule>();
            services.AddScoped<IRule, RemovePupilOtherInPrisonRemandCentreSecureUnitRule>();
            services.AddScoped<IRule, RemovePupilOtherPermanentlyExcluded>();

            services.AddScoped<IAmendmentBuilder, RemovePupilAmendmentBuilder>();
            services.AddScoped<Amendment, RemovePupilAmendment>();

            services.AddScoped<IEstablishmentService, EstablishmentService>();
            services.AddScoped<IPupilService, PupilService>();

            services.AddScoped<IAmendmentService, CrmAmendmentService>();
            services.AddScoped<IOutcomeService, OutcomeService>();
            services.AddScoped<IConfirmationService, CrmConfirmationService>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // This needs to come before swagger
            if (!_env.IsEnvironment(Program.LOCAL_ENVIRONMENT)) app.UseBasicAuth();
            ;

            app.UseSwagger();
            app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Review school and college data API V1");
                    c.RoutePrefix = string.Empty;
                }
            );

            if (_env.IsEnvironment(Program.LOCAL_ENVIRONMENT) || env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAzureAppConfiguration();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
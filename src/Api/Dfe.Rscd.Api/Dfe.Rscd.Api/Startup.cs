using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories;
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
using System.Threading.Tasks;

namespace Dfe.Rscd.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Review school and college data API - V1",
                        Version = "V1",
                        Description = "Provides a restful API for intergating with the RSCD Dynamics CRM Common Data Service and CosmosDB data sources",
                        License = new OpenApiLicense
                        {
                            Name = "MIT License",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        }
                    })
            );

            // Dynamics 365 configuration
            var dynamicsConnString = Configuration.GetConnectionString("DynamicsCds");
            var cdsClient = new CdsServiceClient(dynamicsConnString);

            services.AddTransient<IOrganizationService, CdsServiceClient>(sp => cdsClient.Clone());
            services.Configure<DynamicsOptions>(Configuration.GetSection("Dynamics"));


            services.Configure<BasicAuthOptions>(Configuration.GetSection("BasicAuth"));
            var cosmosDbOptions = Configuration.GetSection("CosmosDb").Get<CosmosDbOptions>();
            var client = new CosmosClient(cosmosDbOptions.Account, cosmosDbOptions.Key);

            services.AddSingleton(IntialiseEstablishmentService(client, cosmosDbOptions.Database, cosmosDbOptions.EstablishmentsCollection).GetAwaiter().GetResult());
            services.AddSingleton<IEstablishmentService, EstablishmentService>();
            services.AddSingleton<IAmendmentService, CrmAmendmentService>();

        }

        private static async Task<IReadRepository<EstablishmentsDTO>> IntialiseEstablishmentService(CosmosClient client, string database, string collection)
        {
            return new EstablishmentRepository(client, database, collection);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Review school and college data API V1");
                c.RoutePrefix = string.Empty;
            }
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseBasicAuth();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

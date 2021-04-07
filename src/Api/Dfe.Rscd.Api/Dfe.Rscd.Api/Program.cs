using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Dfe.Rscd.Api
{
    public class Program
    {
        public const string LOCAL_ENVIRONMENT = "Local";

        public static void Main(string[] args)
        {
            try
            {
                using IHost host = CreateHostBuilder(args).Build();
                host.Run();
            }
            catch (Exception ex)
            {
                // Log.Logger will likely be internal type "Serilog.Core.Pipeline.SilentLogger".
                if (Log.Logger == null || Log.Logger.GetType().Name == "SilentLogger")
                {
                    // Loading configuration or Serilog failed.
                    // Ensure application logs are enabled in Azure App Service to capture
                    // these start-up errors.
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.Console()
                        .CreateLogger();
                }

                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            config.AddJsonFile("collection_lookup.json");

                            if (hostingContext.HostingEnvironment.IsEnvironment(LOCAL_ENVIRONMENT))
                            {
                                config.AddUserSecrets<Program>();
                            }

                            var settings = config.Build();
                            var configLabel = settings["ConfigLabel"];
                            config.AddAzureAppConfiguration(options =>
                            {
                                options.Connect(settings["ConnectionStrings:AppConfig"])
                                    .Select(KeyFilter.Any)
                                    .Select(KeyFilter.Any, configLabel)
                                    .UseFeatureFlags();
                            });
                        })
                        .UseSerilog((hostingContext, loggerConfiguration) => {
                            loggerConfiguration
                                .ReadFrom.Configuration(hostingContext.Configuration)
                                .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment.EnvironmentName);
                        })
                        .UseStartup<Startup>();
                });
        }
    }
}
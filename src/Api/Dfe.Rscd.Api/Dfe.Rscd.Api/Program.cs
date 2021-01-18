using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dfe.Rscd.Api
{
    public class Program
    {
        public const string LOCAL_ENVIRONMENT = "Local";

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            if (hostingContext.HostingEnvironment.IsEnvironment(LOCAL_ENVIRONMENT))
                            {
                                config.AddUserSecrets<Program>();
                            }

                            var settings = config.Build();
                            var configLabel = settings["ConfigLabel"];
                            config.AddAzureAppConfiguration(options =>
                            {
                                options.Connect(settings["ConnectionStrings:AppConfig"])
                                    .Select(KeyFilter.Any, LabelFilter.Null)
                                    .Select(KeyFilter.Any, configLabel)
                                    .UseFeatureFlags();
                            });
                        })
                        .UseStartup<Startup>();
                });
    }
}

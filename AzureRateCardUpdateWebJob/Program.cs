using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AzureRateCardUpdateWebJob
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new HostBuilder();
            builder.ConfigureWebJobs(b =>
            {
                b.AddAzureStorageCoreServices();
            });
            builder.ConfigureLogging((context, b) =>
            {
                b.AddConsole();
            });
            var host = builder.Build();
            using (host)
            {
                var jobHost = host.Services.GetService(typeof(IJobHost)) as JobHost;

                host.StartAsync().Wait();
                jobHost.CallAsync("Import").Wait();
                host.StopAsync().Wait();
            }
        }

        [NoAutomaticTrigger]
        public static void Import(ILogger logger)
        {
            logger.LogInformation("Starting Import");
            var p = new AzureRateCardUpload.Program();
            p.ImportAll();
            logger.LogInformation("Stopping Import");
        }

    }
}

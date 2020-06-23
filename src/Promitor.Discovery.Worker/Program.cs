using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Promitor.Discovery.Worker.Discovery;

namespace Promitor.Discovery.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient("Promitor Resource Discovery", client =>
                    {
                        // Provide Promitor User-Agent
                        client.DefaultRequestHeaders.Add("User-Agent", "Promitor Scraper");
                    });
                    services.AddTransient<ResourceDiscoveryClient>();
                    services.AddHostedService<Worker>();
                });
    }
}

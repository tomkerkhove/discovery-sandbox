using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Promitor.Discovery.Worker.Discovery;

namespace Promitor.Discovery.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ResourceDiscoveryClient _resourceDiscoveryClient;
        private readonly ILogger<Worker> _logger;

        public Worker(ResourceDiscoveryClient resourceDiscoveryClient,ILogger<Worker> logger)
        {
            _logger = logger;
            _resourceDiscoveryClient = resourceDiscoveryClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.UtcNow);

                    var containerRegistries = await _resourceDiscoveryClient.GetAsync("container-registry-landscape");
                    _logger.LogInformation("Found {Count} container registries", containerRegistries.Count);

                    var appPlans = await _resourceDiscoveryClient.GetAsync("app-plan-landscape");
                    _logger.LogInformation("Found {Count} app plans", appPlans.Count);

                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Woops!");
            }
        }
    }
}

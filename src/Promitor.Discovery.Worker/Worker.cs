using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Promitor.Discovery.Contracts.ResourceTypes;
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

                    await GetContainerRegistryInfoAsync();
                    await GetAppPlanInfoAsync();

                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Woops!");
            }
        }

        private async Task GetAppPlanInfoAsync()
        {
            var appPlans = await _resourceDiscoveryClient.GetAsync("app-plan-landscape");
            _logger.LogInformation("Found {Count} app plans", appPlans.Count);

            foreach (var entry in appPlans)
            {
                if (entry is AppPlanResourceDefinition appPlanResource)
                {
                    _logger.LogInformation("Found {AppPlan} app plan", appPlanResource.AppPlanName);
                }
            }
        }

        private async Task GetContainerRegistryInfoAsync()
        {
            var containerRegistries = await _resourceDiscoveryClient.GetAsync("container-registry-landscape");
            _logger.LogInformation("Found {Count} container registries", containerRegistries.Count);

            foreach (var entry in containerRegistries)
            {
                if(entry is ContainerRegistryResourceDefinition containerRegistryResource)
                {
                    _logger.LogInformation("Found {RegistryName} container registry", containerRegistryResource.RegistryName);
                }
            }
        }
    }
}

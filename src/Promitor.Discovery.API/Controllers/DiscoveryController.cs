using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Promitor.Discovery.Contracts.ResourceTypes;

namespace Promitor.Discovery.API.Controllers
{
    /// <summary>
    /// API endpoint to discover Azure resources
    /// </summary>
    [ApiController]
    [Route("api/v1/resources/collections")]
    public class DiscoveryController : ControllerBase
    {
        /// <summary>
        ///     Discover Resources
        /// </summary>
        /// <remarks>Discovers Azure resources matching the criteria.</remarks>
        [HttpGet("{resourceCollectionName}/discovery", Name = "Discovery_Get")]
        public IActionResult Get(string resourceCollectionName)
        {
            if (resourceCollectionName == "app-plan-landscape")
            {
                var resource = new List<AppPlanResourceDefinition>
                {
                    new AppPlanResourceDefinition("ABC", "123", "app-plan-1"),
                    new AppPlanResourceDefinition("ABC", "123", "app-plan-2")
                };
                return Ok(resource);
            }

            if (resourceCollectionName == "container-registry-landscape")
            {
                var resource = new List<ContainerRegistryResourceDefinition>
                {
                    new ContainerRegistryResourceDefinition("ABC", "123", "registry-1"),
                    new ContainerRegistryResourceDefinition("ABC", "123", "registry-2")
                };
                return Ok(resource);
            }

            return NotFound();
        }
    }
}

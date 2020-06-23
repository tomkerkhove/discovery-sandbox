using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Promitor.Discovery.Contracts;

namespace Promitor.Discovery.Worker.Discovery
{
    public class ResourceDiscoveryClient
    {
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Objects};
        private readonly IHttpClientFactory _httpClientFactory;

        public ResourceDiscoveryClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<AzureResourceDefinition>> GetAsync(string resourceCollectionName)
        {
            var uri = $"/api/v1/resources/collections/{resourceCollectionName}/discovery";
            var rawResponse = await SendGetRequestAsync(uri);

            var foundResources = JsonConvert.DeserializeObject<List<AzureResourceDefinition>>(rawResponse, _serializerSettings);
            return foundResources;
        }

        private async Task<string> SendGetRequestAsync(string uri)
        {
            var client = CreateHttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var rawResponse = await response.Content.ReadAsStringAsync();
            return rawResponse;
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient("Promitor Resource Discovery");
            httpClient.BaseAddress = new Uri("http://promitor.discovery.api:80");
            return httpClient;
        }
    }
}

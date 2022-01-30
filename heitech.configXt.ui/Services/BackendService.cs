using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using heitech.configXt.ui;

namespace todo_list_frontend.Application
{
    public class BackendService : IBackendService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpConfiguration _configuration;

        public BackendService(HttpClient httpClient, HttpConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        private async Task RunHttpCommandAsync<T>(T entity, string httpMethod)
        {
            string endpoint = entity.GetType().Name;
            var body = JsonContent.Create(entity);
            var requestMessage = new HttpRequestMessage()
            {
                Method = new HttpMethod(httpMethod),
                RequestUri = new Uri(_configuration.BaseUri.AbsoluteUri + $"/{endpoint}"),
                Content = body
            };
            requestMessage.Headers.Add("Access-Control-Allow-Origin", "*");
            requestMessage.Headers.Add("Access-Control-Allow-Credentials", "true");
            requestMessage.Headers.Add("Access-Control-Allow-Headers", "Access-Control-Allow-Origin,Content-Type");

            _ = await _httpClient.SendAsync(requestMessage);
        }

        public Task PostAsync<T>(T entity) where T : class
        {
            return RunHttpCommandAsync(entity, "POST");
        }

        public async Task<T> GetAsync<T>(string key) where T : class
        {
            string endpoint = typeof(T).Name;
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint + "/" + key);
            string content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(content);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        {
            string endpoint = typeof(T).Name;
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
            string content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<IEnumerable<T>>(content);
        }

        public Task PutAsync<T>(T entity) where T : class
        {
            return RunHttpCommandAsync(entity, "PUT");
        }

        public Task DeleteAsync<T>(T entity) where T : class
        {
            return RunHttpCommandAsync(entity, "DELETE");
        }
    }
}
using System.Text;
using System.Text.Json;
using BlazorProducts.Client.Features;
using BlazorProducts.Client.Pages;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.WebUtilities;

namespace BlazorProducts.Client.HttpRepository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public OrderRepository(HttpClient client) {
            _client = client; 
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};
        }

        public async Task<List<Order>> GetOrders()
        {
            var response = await _client.GetAsync("orders");
            var content = await response.Content.ReadAsStringAsync();
            if(!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var Items = JsonSerializer.Deserialize<List<Order>>(content, _options);
            return Items;
        }

        public async Task CreateOrder(OrderForCreatingDto order)
        {
            var content = JsonSerializer.Serialize(order);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            var postResult = await _client.PostAsync("orders", bodyContent);
            var postContent = await postResult.Content.ReadAsStringAsync();

            if (!postResult.IsSuccessStatusCode) { throw new ApplicationException(postContent); }
        }
        public async Task UpdateStatus(Guid orderId, int status)
        {
            var uri = $"orders/{orderId}/changestatus/{status}";
            var response = await _client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
        }
    }
}

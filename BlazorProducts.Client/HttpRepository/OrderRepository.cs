using System.Text;
using System.Text.Json;
using BlazorProducts.Client.Features;
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
    }
}

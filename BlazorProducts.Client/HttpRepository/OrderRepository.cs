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

        public async Task<PagingResponse<Order>> GetOrders(ProductParameters productParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageSize"] = productParameters.PageSize.ToString(),
                ["pageNumber"] = productParameters.PageNumber.ToString(),
                ["searchTerm"] = productParameters.SearchTerm == null ? "" : productParameters.SearchTerm,
                ["orderBy"] = productParameters.OrderBy
            };
            var response = await _client.GetAsync(QueryHelpers.AddQueryString("orders", queryStringParam));
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var pagingResponse = new PagingResponse<Order>
            {
                Items = JsonSerializer.Deserialize<List<Order>>(content, _options),
                MetaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First(), _options)
            };
            return pagingResponse;
        }

        public async Task<Order> GetOrder(Guid OrderId)
        {
            var url = Path.Combine("Orders", OrderId.ToString());
            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var order = JsonSerializer.Deserialize<Order>(content, _options);
            return order;
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
        public async Task<Order> UpdateOrder(Guid orderId, OrderForCreatingDto order)
        {
            var content = JsonSerializer.Serialize(order);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

            var postResult = await _client.PutAsync($"orders/{orderId}", bodyContent);
            var postContent = await postResult.Content.ReadAsStringAsync();

            if (!postResult.IsSuccessStatusCode) { throw new ApplicationException(postContent); }

            var NewOrder = JsonSerializer.Deserialize<Order>(postContent, _options);
            return NewOrder;
        }
    }
}

using BlazorProducts.Client.Features;
using BlazorProducts.Client.Pages;
using Entities.Models;
using Entities.RequestFeatures;

namespace BlazorProducts.Client.HttpRepository
{
    public interface IOrderRepository
    {
        Task<PagingResponse<Order>> GetOrders(ProductParameters productParameters);
        Task<Order> GetOrder(Guid OrderId);
        Task CreateOrder(OrderForCreatingDto order);
        Task<Order> UpdateOrder(Guid orderId, OrderForCreatingDto order);
        Task UpdateStatus(Guid orderId, int status);
    }
}

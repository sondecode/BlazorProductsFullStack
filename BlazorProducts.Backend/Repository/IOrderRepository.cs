using BlazorProducts.Backend.Paging;
using Entities.Models;
using Entities.RequestFeatures;

namespace BlazorProducts.Backend.Repository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrders();
        Task CreateOrder(OrderForCreatingDto order);
        Task<Order> GetOrder(Guid orderId);
        Task UpdateStatus(Guid orderId, OrderStatus status);
    }
}

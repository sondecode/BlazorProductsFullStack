using BlazorProducts.Client.Features;
using BlazorProducts.Client.Pages;
using Entities.Models;
using Entities.RequestFeatures;

namespace BlazorProducts.Client.HttpRepository
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrders();
        Task CreateOrder(OrderForCreatingDto order);
    }
}

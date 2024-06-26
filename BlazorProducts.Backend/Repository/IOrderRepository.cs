﻿using BlazorProducts.Backend.Paging;
using Entities.Models;
using Entities.RequestFeatures;

namespace BlazorProducts.Backend.Repository
{
    public interface IOrderRepository
    {
        Task<PagedList<Order>> GetOrders(ProductParameters productParameters);
        Task CreateOrder(OrderForCreatingDto order);
        Task<Order> GetOrder(Guid orderId);
        Task<Order> UpdateOrder(Guid orderId, OrderForCreatingDto order);
        Task UpdateStatus(Guid orderId, OrderStatus status);
    }
}

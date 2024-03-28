using BlazorProducts.Backend.Paging;
using BlazorProducts.Backend.Repository.RepoExtensions;
using BlazorProducts.Server.Context;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BlazorProducts.Backend.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ProductContext _context;
        public OrderRepository(ProductContext context)
        {
            _context = context;
        }
        public async Task<PagedList<Order>> GetOrders(ProductParameters productParameters)
        {
            var orders = await _context.Orders.Include(o => o.OrderProducts).ThenInclude(op => op.Product).Search(productParameters.SearchTerm)
                .Sort(productParameters.OrderBy)
                .ToListAsync();

            return PagedList<Order>
                .ToPagedList(orders, productParameters.PageNumber, productParameters.PageSize);
        }

        public async Task CreateOrder(OrderForCreatingDto order)
        {
            if (!order.OrderProducts.Any())
            {
                throw new ArgumentException("Invalid order input");
            }
            double total = 0;
            var products = new List<OrderProduct>();
            foreach (var product in order.OrderProducts)
            {
                var item = await _context.Products.FindAsync(product.Id);
                products.Add(new OrderProduct
                {
                    Product = item,
                    Quantity = product.Quantity,
                }) ;
                total += item.Price * product.Quantity;
            }
           

            var NewOrder = new Order
            {
                CustomerName = order.CustomerName,
                OrderDate = order.OrderDate,
                OrderProducts = products,
                OrderTotal = total,
            };
            _context.Add(NewOrder);
            await _context.SaveChangesAsync();
        }
        public async Task<Order> UpdateOrder(Guid orderId, OrderForCreatingDto order)
        {
            var item = await _context.Orders.Include(o => o.OrderProducts).FirstOrDefaultAsync(o => o.Id == orderId);
            if (item == null || !order.OrderProducts.Any())
            {
                throw new ArgumentException("Invalid order input");
            }

            double total = 0;
            var products = new List<OrderProduct>();
            foreach (var product in order.OrderProducts)
            {
                var productItem = await _context.Products.FindAsync(product.Id);
                products.Add(new OrderProduct
                {
                    Product = productItem,
                    Quantity = product.Quantity,
                });
                total += productItem.Price * product.Quantity;
            }

            // Update existing OrderProducts with new ones
            item.OrderProducts.Clear();
            item.CustomerName = order.CustomerName;
            item.OrderDate = order.OrderDate;
            item.OrderProducts = products;
            item.OrderTotal = total;
            await _context.SaveChangesAsync();
            return item;
        }
        public async Task UpdateStatus(Guid orderId, OrderStatus status)
        {
            var item = await _context.Orders.FindAsync(orderId);
            if (item == null)
            {
                throw new ArgumentException("Invalid order input");
            }
            item.Status = status;
            await _context.SaveChangesAsync();
        }
        public async Task<Order> GetOrder(Guid orderId)
        {
            var item = await _context.Orders.Include(o => o.OrderProducts).ThenInclude(op => op.Product).FirstOrDefaultAsync(o => o.Id == orderId);
            if (item == null)
            {
                throw new ArgumentException("Invalid order input");
            }
            return item;
        }

    }
}

using BlazorProducts.Backend.Paging;
using BlazorProducts.Server.Context;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace BlazorProducts.Backend.Repository
{
    public class OrderProductRepository : IOrderProductRepository
    {
        private readonly ProductContext _context;
        public OrderProductRepository(ProductContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<OrderProduct>> GetOrderProducts()
        {
            var orderProduct = await _context.OrderProducts.Include(o => o.Product)
                .ToListAsync();

            return orderProduct;
        }
    }
}

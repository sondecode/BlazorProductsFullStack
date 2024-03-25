using BlazorProducts.Backend.Paging;
using Entities.Models;
using Entities.RequestFeatures;

namespace BlazorProducts.Backend.Repository
{
    public interface IOrderProductRepository
    {
        Task<IEnumerable<OrderProduct>> GetOrderProducts();
    }
}

using BlazorProducts.Backend.Paging;
using Entities.Models;
using Entities.RequestFeatures;

namespace BlazorProducts.Backend.Repository
{
    public interface IProductRepository
    {
        Task<PagedList<Product>> GetProducts(ProductParameters productParameters);
        Task CreateProduct(Product product);
        Task<Product> GetProduct(Guid id);

        Task UpdateProduct(Product product, Product dbProduct);
    }
}

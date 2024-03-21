using BlazorProducts.Client.Features;
using BlazorProducts.Client.Pages;
using Entities.Models;
using Entities.RequestFeatures;

namespace BlazorProducts.Client.HttpRepository
{
    public interface IProductHttpRepository
    {
        Task<PagingResponse<Product>> GetProducts(ProductParameters productParameters);
        Task CreateProduct(Product product);
        Task<string> UploadProductImage(MultipartFormDataContent content);
    }
}

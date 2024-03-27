using BlazorProducts.Client.HttpRepository;
using Entities.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorProducts.Client.Pages
{
    public partial class ProductDetails
    {
        public Product Product { get; set; } = new Product { Name = "", Supplier="",ImageUrl=""};
        [Inject]
        public IProductHttpRepository Repository { get; set; }
        [Parameter]
        public Guid ProductId { get; set; }
        protected async override Task OnInitializedAsync()
        {
            Product = await Repository.GetProduct(ProductId);
        }
    }
}

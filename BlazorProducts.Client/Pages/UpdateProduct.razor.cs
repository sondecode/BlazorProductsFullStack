using BlazorProducts.Client.HttpRepository;
using BlazorProducts.Client.Shared;
using Entities.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorProducts.Client.Pages
{
    public partial class UpdateProduct
    {
        private Product _product;
        private SuccessNotification _notification;
        [Parameter]
        public string Id { get; set; }

        [Inject]
        IProductHttpRepository ProductRepo { get; set; }
        
        protected  async override Task OnInitializedAsync()
        {
            _product = await ProductRepo.GetProduct(new Guid(Id));
        }
        private async Task Update()
        {
            await ProductRepo.UpdateProduct(_product);
            _notification.Show();
        }

        private void AssignImageUrl(string imageUrl) => _product.ImageUrl = imageUrl;


    }
}

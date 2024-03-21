using BlazorProducts.Client.HttpRepository;
using BlazorProducts.Client.Shared;
using Entities.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorProducts.Client.Pages
{
	public partial class CreateProduct
	{

        private SuccessNotification _notification;
        private void AssignImageUrl(string imgUrl) => _product.ImageUrl = imgUrl;
        private Product _product = new Product
		{
			Id = Guid.NewGuid(),
			Name = "",
			Supplier = "",
			Price = 0,
			ImageUrl = ""
		};
		[Inject]
		public IProductHttpRepository ProductRepo { get; set; }
		private async Task Create()
		{
			await ProductRepo.CreateProduct(_product);
            _notification.Show();
        }
	}
}

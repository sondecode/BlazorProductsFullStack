using BlazorProducts.Client.HttpRepository;
using BlazorProducts.Client.Shared;
using Entities.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorProducts.Client.Pages
{
	public partial class CreateProduct
	{
        [Inject]
        public NavigationManager NavManager { get; set; }
        private void SetImgUrl(string imgUrl) => _product.ImageUrl = imgUrl;
        private Product _product = new Product
		{
			Id = Guid.NewGuid(),
			Name = "",
			Supplier = "",
			Price = 0,
			ImageUrl = ""
		};
        [Inject]
        protected IDialogService DialogService { get; set; }
        [Inject]
		public IProductHttpRepository ProductRepo { get; set; }
		private async Task Create()
		{
			await ProductRepo.CreateProduct(_product);
            await ExecuteDialog();
        }
        private async Task ExecuteDialog()
        {
            var parameters = new DialogParameters
        {
            { "Content", "You have successfully created a new product." },
            { "ButtonColor", Color.Primary },
            { "ButtonText", "OK" }
        };
            var dialog = DialogService.Show<DialogNotification>("Success", parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                bool.TryParse(result.Data.ToString(), out bool shouldNavigate);
                if (shouldNavigate)
                    NavManager.NavigateTo("/products");
            }
        }
    }
}

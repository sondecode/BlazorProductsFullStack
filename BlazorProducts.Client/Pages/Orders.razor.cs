using BlazorProducts.Client.HttpRepository;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Components;

namespace BlazorProducts.Client.Pages
{
	public partial class Orders
	{
		public List<Order> OrderList { get; set; } = new List<Order>();

		[Inject]
		public IOrderRepository OrderRepo { get; set; }

		protected async override Task OnInitializedAsync()
		{
			await GetOrders();
		}

		public async Task GetOrders()
		{
            OrderList = await OrderRepo.GetOrders();
        }

    }
}

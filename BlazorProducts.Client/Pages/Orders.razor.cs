using BlazorProducts.Client.HttpRepository;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Components;

namespace BlazorProducts.Client.Pages
{
	public partial class Orders
	{
		public List<Order> OrderList { get; set; } = new List<Order>();
        [Parameter] public Guid OrderId { get; set; }

        [Inject]
		public IOrderRepository OrderRepo { get; set; }


        private async Task UpdateOrderStatus(Order order, int newStatus)
        {
            order.Status = (OrderStatus)newStatus;
            var orderToUpdate = OrderList.FirstOrDefault(o => o.Id == order.Id);
            await OrderRepo.UpdateStatus(order.Id, newStatus);
        }

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

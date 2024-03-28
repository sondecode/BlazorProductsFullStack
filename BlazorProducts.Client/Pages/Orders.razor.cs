using BlazorProducts.Client.Dialog;
using BlazorProducts.Client.HttpRepository;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorProducts.Client.Pages
{
	public partial class Orders
	{
        MudDataGrid<Order> _dataGrid;
        private ProductParameters _productParameters = new ProductParameters();
        [Parameter] public Guid OrderId { get; set; }

        [Inject]
		public IOrderRepository OrderRepo { get; set; }

        private readonly int[] _pageSizeOption = { 4, 6, 10 };

        private async Task<GridData<Order>> GetServerData(GridState<Order> state)
        {
            _productParameters.PageSize = state.PageSize;
            _productParameters.PageNumber = state.Page + 1;


            var response = await OrderRepo.GetOrders(_productParameters);
            return new GridData<Order>
            {
                Items = response.Items,
                TotalItems = response.MetaData.TotalCount
            };
        }
        private void OnSearch(string searchTerm)
        {
            _productParameters.SearchTerm = searchTerm;
            _dataGrid.ReloadServerData();
        }
        private async Task UpdateOrderStatus(Order order, int newStatus)
        {
            order.Status = (OrderStatus)newStatus;
            await OrderRepo.UpdateStatus(order.Id, newStatus);
        }
        private void OpenOrderDetailDialog(Guid orderId)
        {
            var parameters = new DialogParameters<OrderDetail>();
            parameters.Add(x => x.OrderId, orderId);
            DialogService.Show<OrderDetail>("Order Detail", parameters, new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true });
        }
    }
}

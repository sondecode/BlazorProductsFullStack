using BlazorProducts.Client.HttpRepository;
using Entities.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Reflection.Metadata;

namespace BlazorProducts.Client.Dialog
{
    public partial class OrderDetail
    {
        [Inject]
        public IOrderRepository OrderRepo { get; set; }
        [Parameter]
        public Guid OrderId { get; set; }
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }
        public Order OrderData { get; set; }

        void Close() => MudDialog.Close(DialogResult.Ok(true));
        protected override async Task OnInitializedAsync()
        {
            OrderData = await OrderRepo.GetOrder(OrderId);
        }
    }
}

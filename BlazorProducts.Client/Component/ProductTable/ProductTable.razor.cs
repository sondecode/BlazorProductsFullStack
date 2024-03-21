using Entities.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorProducts.Client.Component.ProductTable
{
    public partial class ProductTable
    {
        [Parameter]
        public List<Product> Products { get; set; }
    }
}

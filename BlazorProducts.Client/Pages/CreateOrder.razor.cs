using BlazorProducts.Client.HttpRepository;
using BlazorProducts.Client.Shared;
using Entities.DTO;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Components;

namespace BlazorProducts.Client.Pages
{
    public partial class CreateOrder
    {
        private SuccessNotification _notification;

        private OrderForCreatingDto Order { get; set; } = new OrderForCreatingDto { OrderDate = DateTime.Now.Date };
        private List<CartItem> ShoppingCart { get; set; } = new List<CartItem>();

        public List<Product> SearchProductList { get; set; } = new List<Product>();
        private bool ShowDropdown { get; set; }
        public MetaData MetaData { get; set; } = new MetaData();
        private ProductParameters _productParameters = new ProductParameters();
        [Inject]
        public IProductHttpRepository ProductRepo { get; set; }
        [Inject]
        public IOrderRepository OrderRepo { get; set; }

        private async Task SearchChanged(string searchTerm)
        {
            if (searchTerm == "") ShowDropdown = false;
            else
            {

                _productParameters.PageNumber = 1;
                _productParameters.SearchTerm = searchTerm;
                await GetProducts();
            }
        }

        private void SelectProduct(Product product)
        {
            // Handle selection of a product from the dropdown
            // Example: Do something with the selected product
            Console.WriteLine($"Selected product: {product.Name}");

            // Reset search state
            AddToCart(product);
            SearchProductList = null;
            ShowDropdown = false;
        }

        private async Task GetProducts()
        {
            var pagingResponse = await ProductRepo.GetProducts(_productParameters);
            SearchProductList = pagingResponse.Items;
            MetaData = pagingResponse.MetaData;
            ShowDropdown = SearchProductList != null && SearchProductList.Any();
        }

        private async Task CreateOrderForm()
        {
            Order.OrderProducts = ConvertShoppingCartToDto(ShoppingCart);
            await OrderRepo.CreateOrder(Order);
            _notification.Show();
        }

        // Method to add a product to the shopping cart
        private void AddToCart(Product product)
        {
            // Check if the product is already in the cart
            var existingItem = ShoppingCart.FirstOrDefault(item => item.Id == product.Id);
            if (existingItem != null)
            {
                // Increment quantity if the product is already in the cart
                existingItem.Quantity++;
            }
            else
            {
                // Add the product to the cart with quantity 1
                ShoppingCart.Add(new CartItem
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Quantity = 1
                });
            }
        }
        private void UpdateQuantity(CartItem item)
        {
            // Ensure the quantity is not negative
            if (item.Quantity < 0)
            {
                item.Quantity = 0;
            }
        }
        private void RemoveFromCart(CartItem item)
        {
            ShoppingCart.Remove(item);
        }

        private ICollection<OrderProductForCreatingDto> ConvertShoppingCartToDto(List<CartItem> ShoppingCart)
        {
            List<OrderProductForCreatingDto> orderProducts = new List<OrderProductForCreatingDto>();

            foreach (var item in ShoppingCart)
            {
                orderProducts.Add(new OrderProductForCreatingDto
                {
                    Id = item.Id,
                    Quantity = item.Quantity
                });
            }

            return orderProducts;
        }
    }
}

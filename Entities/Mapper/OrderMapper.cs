using Entities.DTO;
using Entities.Models;

namespace Entities.Mapper
{
    public static class OrderMapper
    {
        public static OrderForCreatingDto MapToOrderForCreatingDto(Order order)
        {
            if (order == null)
                return null;

            return new OrderForCreatingDto
            {
                CustomerName = order.CustomerName,
                OrderDate = order.OrderDate,
                OrderProducts = MapToOrderProductsForCreatingDto(order.OrderProducts)
                // Map other properties as needed
            };
        }

        public static OrderProductForCreatingDto MapToOrderProductForCreatingDto(OrderProduct orderProduct)
        {
            if (orderProduct == null)
                return null;

            return new OrderProductForCreatingDto
            {
                Id = orderProduct.Id,
                Quantity = orderProduct.Quantity
                // Map other properties as needed
            };
        }

        public static ICollection<OrderProductForCreatingDto> MapToOrderProductsForCreatingDto(ICollection<OrderProduct> orderProducts)
        {
            if (orderProducts == null)
                return null;

            var orderProductsForCreatingDto = new List<OrderProductForCreatingDto>();

            foreach (var orderProduct in orderProducts)
            {
                var orderProductForCreatingDto = MapToOrderProductForCreatingDto(orderProduct);
                orderProductsForCreatingDto.Add(orderProductForCreatingDto);
            }

            return orderProductsForCreatingDto;
        }
        public static CartItem MapToCartItem(OrderProduct orderProduct)
        {
            if (orderProduct == null)
                return null;

            return new CartItem
            {
                Id = orderProduct.Product.Id,
                Name = orderProduct.Product.Name,
                Quantity = orderProduct.Quantity,
                Price = orderProduct.Product.Price,
                ImageUrl = orderProduct.Product.ImageUrl,
                // Map other properties as needed
            };
        }

        public static List<CartItem> MapToCartItems(ICollection<OrderProduct> orderProducts)
        {
            if (orderProducts == null)
                return null;

            var cartItems = new List<CartItem>();

            foreach (var orderProduct in orderProducts)
            {
                var cartItem = MapToCartItem(orderProduct);
                cartItems.Add(cartItem);
            }

            return cartItems;
        }
    }
}

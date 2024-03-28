using BlazorProducts.Backend.Repository;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlazorProducts.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repo;
        public OrdersController (IOrderRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ProductParameters productParameters)
        {
            var orders = await _repo.GetOrders(productParameters);
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(orders.MetaData));
            return Ok(orders);
        }
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            var orders = await _repo.GetOrder(orderId);
            return Ok(orders);
        }
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(Guid orderId, [FromBody] OrderForCreatingDto orderForCreating)
        {
            var orders = await _repo.UpdateOrder(orderId, orderForCreating);
            return Ok(orders);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderForCreatingDto orderForCreating)
        {
            if (orderForCreating == null)
            {
                return BadRequest();
            }

            await _repo.CreateOrder(orderForCreating);
            return Created("", orderForCreating);
        }
        [HttpGet("{orderId}/ChangeStatus/{status}")]
        public async Task<IActionResult> UpdateStatus(Guid orderId, OrderStatus status)
        {
            await _repo.UpdateStatus(orderId, status);
            return Ok();
        }

    }
}

using BlazorProducts.Backend.Repository;
using BlazorProducts.Server.Context.Configuration;
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
        private IResponseCacheService _responseCacheService;
        private readonly IOrderRepository _repo;
        public OrdersController (IOrderRepository repo, IResponseCacheService responseCacheService)
        {
            _repo = repo;
            _responseCacheService = responseCacheService;
        }
        [HttpGet]

        [Cache(1000, true)]
        public async Task<IActionResult> Get([FromQuery] ProductParameters productParameters)
        {
            var orders = await _repo.GetOrders(productParameters);
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(orders.MetaData));
            return Ok(orders);
        }
        [HttpGet("{orderId}")]
        [Cache(1000)]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            var orders = await _repo.GetOrder(orderId);
            return Ok(orders);
        }
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(Guid orderId, [FromBody] OrderForCreatingDto orderForCreating)
        {
            var orders = await _repo.UpdateOrder(orderId, orderForCreating);
            await _responseCacheService.RemoveCacheResponseAsync($"/orders/{orderId}");
            return Ok(orders);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderForCreatingDto orderForCreating)
        {
            if (orderForCreating == null)
            {
                return BadRequest();
            }
            await _responseCacheService.RemoveCacheResponseAsync($"/orducts");
            await _repo.CreateOrder(orderForCreating);
            return Created("", orderForCreating);
        }
        [HttpGet("{orderId}/ChangeStatus/{status}")]
        public async Task<IActionResult> UpdateStatus(Guid orderId, OrderStatus status)
        {
            await _responseCacheService.RemoveCacheResponseAsync($"/orders/{orderId}");
            await _repo.UpdateStatus(orderId, status);
            return Ok();
        }

    }
}

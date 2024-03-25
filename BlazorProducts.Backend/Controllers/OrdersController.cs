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
        public async Task<IActionResult> Get()
        {
            var orders = await _repo.GetOrders();
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

    }
}

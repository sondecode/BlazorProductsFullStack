using BlazorProducts.Backend.Repository;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlazorProducts.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderProductsController : ControllerBase
    {
        private readonly IOrderProductRepository _repo;
        public OrderProductsController(IOrderProductRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var orders = await _repo.GetOrderProducts();
            return Ok(orders);
        }

    }
}

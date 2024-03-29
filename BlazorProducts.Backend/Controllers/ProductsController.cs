using BlazorProducts.Backend.Repository;
using BlazorProducts.Server.Context.Configuration;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlazorProducts.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class ProductsController : ControllerBase
    {
        private IResponseCacheService _responseCacheService;
        private readonly IProductRepository _repo;
        public ProductsController(IProductRepository repo, IResponseCacheService responseCacheService)
        {
            _repo = repo;
            _responseCacheService = responseCacheService;
        }
        [HttpGet]
        [Authorize]
        [Cache(1000, true)]
        public async Task<IActionResult> Get([FromQuery] ProductParameters productParameters)
        {
            var products = await _repo.GetProducts(productParameters);
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(products.MetaData));
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            await _responseCacheService.RemoveCacheResponseAsync("/Products");
            await _repo.CreateProduct(product);
            return Created("", product);
        }

        [HttpGet("{id}")]
        [Cache(1000)]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _repo.GetProduct(id);
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] Product product)
        {
            //additional product and model validation checks

            var dbProduct = await _repo.GetProduct(id);
            if (dbProduct == null)
            {
                return NotFound();
            }

            await _responseCacheService.RemoveCacheResponseAsync($"/Products/{id}");
            await _repo.UpdateProduct(product, dbProduct);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            //additional product and model validation checks

            var dbProduct = await _repo.GetProduct(id);
            if (dbProduct == null)
            {
                return NotFound();
            }
            await _responseCacheService.RemoveCacheResponseAsync($"/Products/{id}");
            await _repo.DeleteProduct(dbProduct);
           
            return NoContent();
        }
    }
}

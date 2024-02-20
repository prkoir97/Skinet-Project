using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController] // Possesses attributes which helps us write less code
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase // Without view suppot (), just returning json data
    {
        private readonly IProductRepository _repo;

        // Constructor Injection: The IProductRepository instance is injected into the ProductsController.
        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            // Async & Await: Allows for non-blocking execution, improving performance and scalability.
            var products = await _repo.GetProductsAsync();

            // Returns the products as an HTTP 200 OK response.
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            // Retrieves a product by its ID from the repository asynchronously.
            var product = await _repo.GetProductByIdAsync(id);

            return product;
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _repo.GetProductBrandsAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductTypes()
        {
            return Ok(await _repo.GetProductTypesAsync());
        }
    }
}

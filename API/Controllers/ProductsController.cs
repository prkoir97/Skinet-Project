using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController] // Possesses attributes which helps us write less code
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase // Without view suppot (), just returning json data
    { 
        // Access to our DB context to allow us to query the database
        private readonly StoreContext _context;  // Private - can't be access outside field  // readonly - unchangable value
        public ProductsController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()       // Async & Await - allows for best performance, scalability
        {
            var products = await _context.Products.ToListAsync(); 

            return products;
        }

         [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return await _context.Products.FindAsync(id);
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.Models;
using ProductsApi.ProductDTOs;
using ProductsApi.Services;

namespace ProductsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // POST: /api/products
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Mapping DTO to domain model; ignoring client-sent fields such as ProductId, CreatedAt, UpdatedAt.
            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock
            };

            var createdProduct = await _productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProductById),
                new { id = createdProduct.ProductId },
                createdProduct);
        }

        // GET: /api/products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        // GET: /api/products/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new { message = "Product not found." });
            return Ok(product);
        }

        // PUT: /api/products/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // First, fetch the existing product.
            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
                return NotFound(new { message = "Product not found." });
            // Update its properties with values from the UpdateProductDto.
            existingProduct.Name = updateProductDto.Name;
            existingProduct.Description = updateProductDto.Description;
            existingProduct.Price = updateProductDto.Price;
            existingProduct.Stock = updateProductDto.Stock;
            existingProduct.UpdatedAt = System.DateTime.UtcNow;

            // Save changes.
            var result = await _productService.UpdateProductAsync(existingProduct);
            if (!result)
                return NotFound();

            return Ok(_productService.GetProductByIdAsync(id).Result);
        }

        // DELETE: /api/products/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
                return NotFound(new { message = "Product not found." });
            return Ok(new { message = "Product deleted successfully", productId = id });
        }

        // PUT: /api/products/decrement-stock/{id}/{quantity}
        [HttpPut("decrement-stock/{id:int}/{quantity:int}")]
        public async Task<IActionResult> DecrementStock(int id, int quantity)
        {
            if (quantity <= 0)
                return BadRequest("Quantity must be greater than zero.");

            var result = await _productService.DecrementStockAsync(id, quantity);
            if (!result)
                return BadRequest("The product does not exist or there is insufficient stock.");

            return Ok(_productService.GetProductByIdAsync(id).Result);
        }

        // PUT: /api/products/add-to-stock/{id}/{quantity}
        [HttpPut("add-to-stock/{id:int}/{quantity:int}")]
        public async Task<IActionResult> IncrementStock(int id, int quantity)
        {
            if (quantity <= 0)
                return BadRequest("Quantity must be greater than zero.");

            var result = await _productService.IncrementStockAsync(id, quantity);
            if (!result)
                return NotFound();

            return Ok(_productService.GetProductByIdAsync(id).Result);
        }
    }
}

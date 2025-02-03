using Microsoft.AspNetCore.Mvc;
using Project_Coffe.DTO;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;

namespace Project_Coffe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        private readonly IWebHostEnvironment _environment;

        public ProductController(IProductService productService, ILogger<ProductController> logger, IWebHostEnvironment environment)
        {
            _productService = productService;
            _logger = logger;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                List<Product> products = (await _productService.GetAllProducts()).ToList();
                _logger.LogInformation("Fetched all products successfully.");
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching products: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching the products.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                Product? product = await _productService.GetProductById(id);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {id} not found.");
                    return NotFound("Product not found.");
                }

                _logger.LogInformation($"Fetched product with ID {id} successfully.");
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching product with ID {id}: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching the product.");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateProductAndUpload(IFormFile imageFile, IFormFile musicFile, [FromForm] CreateProductDTO productDto)
        {
            try
            {
                if (imageFile == null || musicFile == null || imageFile.Length == 0 || musicFile.Length == 0)
                {
                    _logger.LogWarning("Upload failed: One or more files are missing or empty.");
                    return BadRequest("Both image and music files are required.");
                }

                string imagePath = Path.Combine(_environment.WebRootPath, "images", imageFile.FileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                string musicPath = Path.Combine(_environment.WebRootPath, "music", musicFile.FileName);
                using (var stream = new FileStream(musicPath, FileMode.Create))
                {
                    await musicFile.CopyToAsync(stream);
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Product creation failed: Invalid model state.");
                    return BadRequest(ModelState);
                }

                Product product = new Product
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    ImagePath = $"/images/{imageFile.FileName}",
                    MusicPath = $"/music/{musicFile.FileName}"
                };

                await _productService.CreateProduct(product);
                _logger.LogInformation($"Product with ID {product.Id} created successfully.");
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating product: {ex.Message}");
                return StatusCode(500, "An error occurred while creating the product.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO productDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning($"Update failed: Invalid model or ID mismatch for product {id}.");
                    return BadRequest(ModelState);
                }

                Product product = new Product
                {
                    Id = id,
                    Name = productDto.Name,
                    Price = productDto.Price,
                    Description = productDto.Description,
                    Stock = productDto.Stock,
                    ImagePath = productDto.ImagePath,
                    MusicPath = productDto.MusicPath
                };

                await _productService.UpdateProduct(product);
                _logger.LogInformation($"Product with ID {id} updated successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product with ID {id}: {ex.Message}");
                return StatusCode(500, "An error occurred while updating the product.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                _logger.LogInformation($"Product with ID {id} deleted successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting product with ID {id}: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the product.");
            }
        }

    }
}

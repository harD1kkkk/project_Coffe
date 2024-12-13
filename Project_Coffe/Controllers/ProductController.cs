<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Mvc;
=======
﻿using CoffeeShopAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
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

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
<<<<<<< HEAD
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
=======
            var products = await _productService.GetAllProducts();
            return Ok(products);
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
<<<<<<< HEAD
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
=======
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
<<<<<<< HEAD
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Product creation failed: Invalid model state.");
                    return BadRequest(ModelState);
                }

                await _productService.CreateProduct(product);
                _logger.LogInformation($"Product with ID {product.Id} created successfully.");
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating product: {ex.Message}");
                return StatusCode(500, "An error occurred while creating the product.");
            }
=======
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _productService.CreateProduct(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
<<<<<<< HEAD
            try
            {
                if (!ModelState.IsValid || id != product.Id)
                {
                    _logger.LogWarning($"Update failed: Invalid model or ID mismatch for product {id}.");
                    return BadRequest(ModelState);
                }

                await _productService.UpdateProduct(product);
                _logger.LogInformation($"Product with ID {id} updated successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product with ID {id}: {ex.Message}");
                return StatusCode(500, "An error occurred while updating the product.");
            }
=======
            if (!ModelState.IsValid || id != product.Id)
            {
                return BadRequest(ModelState);
            }

            await _productService.UpdateProduct(product);
            return NoContent();
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
<<<<<<< HEAD
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

=======
            await _productService.DeleteProduct(id);
            return NoContent();
        }
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
    }
}

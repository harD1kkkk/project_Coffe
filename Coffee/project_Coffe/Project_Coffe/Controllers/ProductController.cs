using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_Coffe.DTO;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;
using System.Net.Http.Headers;

namespace Project_Coffe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly string? _virusTotalApiKey;

        public ProductController(IProductService productService, ILogger<ProductController> logger, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _productService = productService;
            _logger = logger;
            _environment = environment;
            _virusTotalApiKey = configuration["VirusTotal:ApiKey"];
        }

        [Authorize]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

                if (!await IsFileSafeAsync(imageFile))
                {
                    _logger.LogWarning("Virus scan failed: The image file is infected or unsafe.");
                    return BadRequest("The uploaded image file is unsafe.");
                }
                _logger.LogInformation("Virus scan good: The image file is safe.");

                if (!await IsFileSafeAsync(musicFile))
                {
                    _logger.LogWarning("Virus scan failed: The music file is infected or unsafe.");
                    return BadRequest("The uploaded music file is unsafe.");
                }
                _logger.LogInformation("Virus scan good: The music file is safe.");

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

                string defaultPath = "C:/project_Coffe/Project_Coffe/wwwroot";
                Product product = new Product
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    Description = productDto.Description,
                    Stock = productDto.Stock,
                    ImagePath = $"{defaultPath}/images/{imageFile.FileName}",
                    MusicPath = $"{defaultPath}/music/{musicFile.FileName}"
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

        [Authorize(Roles = "Admin")]
        [HttpPut("update-product/{id}")]
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

        [Authorize(Roles = "Admin")]
        [HttpPut("update-imageFile/{id}")]
        public async Task<IActionResult> UpdateProductImageFile(int id, IFormFile imageFile)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    _logger.LogWarning("Upload failed: Image file is missing or empty.");
                    return BadRequest("Image file is required.");
                }
                if (!await IsFileSafeAsync(imageFile))
                {
                    _logger.LogWarning("Virus scan failed: The image file is infected or unsafe.");
                    return BadRequest("The uploaded image file is unsafe.");
                }
                _logger.LogInformation("Virus scan good: The image file is safe.");

                Product? existingProduct = await _productService.GetProductById(id);
                if (existingProduct == null)
                {
                    _logger.LogWarning($"Product with ID {id} not found.");
                    return NotFound($"Product with ID {id} not found.");
                }

                string imagePath = Path.Combine(_environment.WebRootPath, "images", imageFile.FileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                string defaultPath = "C:/project_Coffe/Project_Coffe/wwwroot";
                existingProduct.ImagePath = $"{defaultPath}/images/{imageFile.FileName}";

                await _productService.UpdateProductImage(id, existingProduct);
                _logger.LogInformation($"Product Image with ID {id} updated successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product image: {ex.Message}");
                return StatusCode(500, "An error occurred while updating the product image.");
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("update-musicFile/{id}")]
        public async Task<IActionResult> UpdateProductMusicFile(int id, IFormFile musicFile)
        {
            try
            {
                if (musicFile == null || musicFile.Length == 0)
                {
                    _logger.LogWarning("Upload failed: Music file is missing or empty.");
                    return BadRequest("Music file is required.");
                }
                if (!await IsFileSafeAsync(musicFile))
                {
                    _logger.LogWarning("Virus scan failed: The music file is infected or unsafe.");
                    return BadRequest("The uploaded music file is unsafe.");
                }
                _logger.LogInformation("Virus scan good: The music file is safe.");

                Product? existingProduct = await _productService.GetProductById(id);
                if (existingProduct == null)
                {
                    _logger.LogWarning($"Product with ID {id} not found.");
                    return NotFound($"Product with ID {id} not found.");
                }

                string musicPath = Path.Combine(_environment.WebRootPath, "music", musicFile.FileName);
                using (var stream = new FileStream(musicPath, FileMode.Create))
                {
                    await musicFile.CopyToAsync(stream);
                }

                string defaultPath = "C:/project_Coffe/Project_Coffe/wwwroot";
                existingProduct.MusicPath = $"{defaultPath}/music/{musicFile.FileName}";

                await _productService.UpdateProductImage(id, existingProduct);
                _logger.LogInformation($"Product Music with ID {id} updated successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product Music: {ex.Message}");
                return StatusCode(500, "An error occurred while updating the product Music.");
            }
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize]
        [HttpPut("search-and-sort")]
        public async Task<IActionResult> SearchAndSort([FromBody] ProductFilterDto filter)
        {
            try
            {
                List<Product> products = (await _productService.SearchAndSort(filter)).ToList();
                if (!products.Any())
                {
                    _logger.LogInformation("No products found matching the provided filters.");
                    return NotFound("No products found matching the provided filters.");
                }

                _logger.LogInformation("Fetched products successfully.");
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching products: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching the products.");
            }
        }
        private async Task<bool> IsFileSafeAsync(IFormFile file)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-apikey", _virusTotalApiKey);

                using (var content = new MultipartFormDataContent())
                {
                    var streamContent = new StreamContent(file.OpenReadStream());
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    content.Add(streamContent, "file", file.FileName);

                    var response = await client.PostAsync("https://www.virustotal.com/api/v3/files", content);
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("Error uploading file to VirusTotal API.");
                        return false;
                    }

                    var responseData = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseData);

                    if (jsonResponse?.data?.id == null)
                    {
                        _logger.LogError("Invalid response from VirusTotal API.");
                        return false;
                    }

                    string analysisId = jsonResponse.data.id;
                    bool isCompleted = false;
                    dynamic analysisResult = null;
                    int count = 0; 

                    while (!isCompleted && count < 10)
                    {
                        var analysisResponse = await client.GetAsync($"https://www.virustotal.com/api/v3/analyses/{analysisId}");

                        if (!analysisResponse.IsSuccessStatusCode)
                        {
                            _logger.LogError("Error retrieving analysis from VirusTotal API.");
                            return false;
                        }

                        var analysisResponseData = await analysisResponse.Content.ReadAsStringAsync();
                        analysisResult = JsonConvert.DeserializeObject(analysisResponseData);

                        if (analysisResult?.data?.attributes?.status == "completed")
                        {
                            isCompleted = true;
                            break;
                        }

                        count++;
                        await Task.Delay(5000);  
                    }

                    if (!isCompleted)
                    {
                        _logger.LogWarning("VirusTotal analysis did not complete in time.");
                        return false;
                    }

                    int maliciousCount = analysisResult.data.attributes.stats.malicious;
                    return maliciousCount == 0;
                }
            }
        }
    }
}

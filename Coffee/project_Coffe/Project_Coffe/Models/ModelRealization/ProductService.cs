using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project_Coffe.Data;
using Project_Coffe.DTO;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;
using System.Net.Http.Headers;

namespace CoffeeShopAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;
        private readonly string? _virusTotalApiKey;

        public ProductService(ApplicationDbContext dbContext, ILogger<ProductService> logger, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _logger = logger;
            _virusTotalApiKey = configuration["VirusTotal:ApiKey"];
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            try
            {
                IEnumerable<Product> products = await _dbContext.Set<Product>().ToListAsync();
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching all products: {ex.Message}");
                throw new Exception("An error occurred while fetching products.");
            }
        }

        public async Task<Product?> GetProductById(int productId)
        {
            try
            {
                Product? product = await _dbContext.Set<Product>().FindAsync(productId);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {productId} not found.");
                }
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching product with ID {productId}: {ex.Message}");
                throw new Exception("An error occurred while fetching the product.");
            }
        }

        public async Task CreateProduct(Product product)
        {
            try
            {
                if (product == null)
                {
                    _logger.LogError("Attempted to create a null product.");
                    throw new ArgumentNullException(nameof(product), "Product cannot be null.");
                }
                if (decimal.Round(product.Price, 2) != product.Price)
                {
                    _logger.LogError("Price must be rounded to two decimal places.");
                    throw new ArgumentException("Price must be rounded to two decimal places.", nameof(product.Price));
                }
                await _dbContext.Set<Product>().AddAsync(product);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Product created with ID: {ProductId}", product.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating product: {ex.Message}");
                throw new Exception("An error occurred while creating the product.");
            }
        }

        public async Task UpdateProduct(Product product)
        {
            try
            {
                if (product == null)
                {
                    _logger.LogError("Attempted to update a null product.");
                    throw new ArgumentNullException(nameof(product), "Product cannot be null.");
                }

                bool existingProduct = await _dbContext.Products.AnyAsync(i => i.Id == product.Id);
                if (!existingProduct)
                {
                    _logger.LogWarning($"Product with ID {product.Id} not found for update.");
                    throw new KeyNotFoundException($"Product with ID {product.Id} not found.");
                }
                if (decimal.Round(product.Price, 2) != product.Price)
                {
                    _logger.LogError("Price must be rounded to two decimal places.");
                    throw new ArgumentException("Price must be rounded to two decimal places.", nameof(product.Price));
                }

                _dbContext.Set<Product>().Update(product);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Product updated with ID: {ProductId}", product.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product with ID {product.Id}: {ex.Message}");
                throw new Exception("An error occurred while updating the product.");
            }
        }

        public async Task UpdateProductImage(int productId, Product product)
        {
            try
            {
                Product? existingProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (existingProduct == null)
                {
                    _logger.LogWarning($"Product with ID {productId} not found.");
                    throw new KeyNotFoundException($"Product with ID {productId} not found.");
                }

                existingProduct.ImagePath = product.ImagePath;

                _dbContext.Products.Update(existingProduct);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product image with ID {productId}: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateProductMusic(int productId, Product product)
        {
            try
            {
                Product? existingProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (existingProduct == null)
                {
                    _logger.LogWarning($"Product with ID {productId} not found.");
                    throw new KeyNotFoundException($"Product with ID {productId} not found.");
                }

                existingProduct.MusicPath = product.MusicPath;

                _dbContext.Products.Update(existingProduct);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product music with ID {productId}: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteProduct(int productId)
        {
            try
            {
                Product? product = await _dbContext.Set<Product>().FindAsync(productId);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {productId} not found for deletion.");
                    throw new KeyNotFoundException($"Product with ID {productId} not found.");
                }

                _dbContext.Set<Product>().Remove(product);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Product deleted with ID: {ProductId}", productId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting product with ID {productId}: {ex.Message}");
                throw new Exception("An error occurred while deleting the product.");
            }
        }

        public async Task<IEnumerable<Product>> SearchAndSort(ProductFilterDto filter)
        {
            try
            {
                IQueryable<Product> products = _dbContext.Set<Product>();

                if (!string.IsNullOrEmpty(filter.Name))
                {
                    products = products.Where(p => p.Name != null && p.Name.Contains(filter.Name));
                }

                if (filter.Price.HasValue)
                {
                    products = products.Where(p => p.Price <= filter.Price);
                }

                if (filter.SortLowOrHighPrice.HasValue)
                {
                    if (filter.SortLowOrHighPrice == true)
                    {
                        products = products.OrderBy(p => p.Price);
                    }
                    else
                    {
                        products = products.OrderByDescending(p => p.Price);
                    }
                }

                return await products.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching filtered products: {ex.Message}");
                throw new Exception("An error occurred while fetching the products.");
            }
        }
        public async Task<bool> IsFileSafeAsync(IFormFile file)
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
                    dynamic? jsonResponse = JsonConvert.DeserializeObject(responseData);

                    if (jsonResponse?.data?.id == null)
                    {
                        _logger.LogError("Invalid response from VirusTotal API.");
                        return false;
                    }

                    string analysisId = jsonResponse.data.id;
                    bool isCompleted = false;
                    dynamic? analysisResult = null;
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

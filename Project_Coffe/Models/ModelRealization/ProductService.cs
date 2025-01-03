using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using Project_Coffe.Data;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;

namespace CoffeeShopAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ApplicationDbContext dbContext, ILogger<ProductService> logger)
=======
using Project_Coffe.Entities;
using System;
using Project_Coffe.Data;
using Project_Coffe.Models.ModelInterface;


namespace CoffeeShopAPI.Services
{
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;

    public class ProductService : IProductService
    {
        private readonly DbContext _dbContext;
        private readonly ILogger<ProductService> _logger;

        public ProductService(DbContext dbContext, ILogger<ProductService> logger)
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
<<<<<<< HEAD
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
=======
            return await _dbContext.Set<Product>().ToListAsync();
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
        }

        public async Task<Product?> GetProductById(int productId)
        {
<<<<<<< HEAD
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
=======
            return await _dbContext.Set<Product>().FindAsync(productId);
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
        }

        public async Task CreateProduct(Product product)
        {
<<<<<<< HEAD
            try
            {
                if (product == null)
                {
                    _logger.LogError("Attempted to create a null product.");
                    throw new ArgumentNullException(nameof(product), "Product cannot be null.");
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
=======
            await _dbContext.Set<Product>().AddAsync(product);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Product created with ID: {ProductId}", product.Id);
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
        }

        public async Task UpdateProduct(Product product)
        {
<<<<<<< HEAD
            try
            {
                if (product == null)
                {
                    _logger.LogError("Attempted to update a null product.");
                    throw new ArgumentNullException(nameof(product), "Product cannot be null.");
                }

                Product? existingProduct = await _dbContext.Set<Product>().FindAsync(product.Id);
                if (existingProduct == null)
                {
                    _logger.LogWarning($"Product with ID {product.Id} not found for update.");
                    throw new KeyNotFoundException($"Product with ID {product.Id} not found.");
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
=======
            _dbContext.Set<Product>().Update(product);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Product updated with ID: {ProductId}", product.Id);
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
        }

        public async Task DeleteProduct(int productId)
        {
<<<<<<< HEAD
            try
            {
                Product? product = await _dbContext.Set<Product>().FindAsync(productId);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {productId} not found for deletion.");
                    throw new KeyNotFoundException($"Product with ID {productId} not found.");
                }

=======
            var product = await _dbContext.Set<Product>().FindAsync(productId);
            if (product != null)
            {
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
                _dbContext.Set<Product>().Remove(product);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Product deleted with ID: {ProductId}", productId);
            }
<<<<<<< HEAD
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting product with ID {productId}: {ex.Message}");
                throw new Exception("An error occurred while deleting the product.");
            }
        }
    }
}
=======
        }
    }
}

>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9

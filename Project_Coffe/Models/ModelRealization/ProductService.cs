using Microsoft.EntityFrameworkCore;
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
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ApplicationDbContext dbContext, ILogger<ProductService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _dbContext.Set<Product>().ToListAsync();
        }

        public async Task<Product?> GetProductById(int productId)
        {
            return await _dbContext.Set<Product>().FindAsync(productId);
        }

        public async Task CreateProduct(Product product)
        {
            await _dbContext.Set<Product>().AddAsync(product);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Product created with ID: {ProductId}", product.Id);
        }

        public async Task UpdateProduct(Product product)
        {
            _dbContext.Set<Product>().Update(product);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Product updated with ID: {ProductId}", product.Id);
        }

        public async Task DeleteProduct(int productId)
        {
            var product = await _dbContext.Set<Product>().FindAsync(productId);
            if (product != null)
            {
                _dbContext.Set<Product>().Remove(product);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Product deleted with ID: {ProductId}", productId);
            }
        }
    }
}


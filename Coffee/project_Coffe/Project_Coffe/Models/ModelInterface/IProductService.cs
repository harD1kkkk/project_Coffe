﻿using Project_Coffe.DTO;
using Project_Coffe.Entities;

namespace Project_Coffe.Models.ModelInterface
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product?> GetProductById(int productId);
        Task CreateProduct(Product product);
        Task UpdateProduct(Product product);
        Task UpdateProductImage(int productId, Product product);
        Task UpdateProductMusic(int productId, Product product);
        Task DeleteProduct(int productId);
        Task<IEnumerable<Product>> SearchAndSort(ProductFilterDto filter);
        Task<bool> IsFileSafeAsync(IFormFile file);
    }
}

using Project_Coffe.Entities;

namespace Project_Coffe.Models.ModelInterface
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product?> GetProductById(int productId);
        Task CreateProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int productId);
    }
<<<<<<< HEAD
=======

>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
}

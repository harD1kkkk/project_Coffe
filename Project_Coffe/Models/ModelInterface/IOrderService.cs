using Project_Coffe.Entities;

namespace Project_Coffe.Models.ModelInterface
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order?> GetOrderById(int orderId);
<<<<<<< HEAD
        Task<Order?> GetActiveOrder(int userId);
        Task<Order> CreateOrder(int userId, List<OrderProduct> orderProducts);
        Task<Order> CreateOrUpdateOrder(int userId, List<OrderProduct> orderProducts);
        Task AddProductsToActiveOrder(int orderId, List<OrderProduct> orderProducts);
        Task UpdateOrder(Order order, List<OrderProduct> orderProducts);
        Task DeleteOrder(int orderId);
    }
=======
        Task CreateOrder(Order order, List<OrderProduct> orderProducts);
        Task UpdateOrder(Order order, List<OrderProduct> orderProducts);
        Task DeleteOrder(int orderId);
    }

>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
}

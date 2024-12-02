using Microsoft.EntityFrameworkCore;
using Project_Coffe.Data;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;

namespace Project_Coffe.Models.ModelRealization
{
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;

    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<OrderService> _logger;

        public OrderService(ApplicationDbContext dbContext, ILogger<OrderService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _dbContext.Set<Order>()
                                   .Include(o => o.OrderProducts)
                                   .ThenInclude(op => op.Product)
                                   .ToListAsync();
        }

        public async Task<Order?> GetOrderById(int orderId)
        {
            return await _dbContext.Set<Order>()
                                   .Include(o => o.OrderProducts)
                                   .ThenInclude(op => op.Product)
                                   .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task CreateOrder(Order order, List<OrderProduct> orderProducts)
        {
            await _dbContext.Set<Order>().AddAsync(order);
            await _dbContext.SaveChangesAsync();

            foreach (var orderProduct in orderProducts)
            {
                orderProduct.OrderId = order.Id;
                await _dbContext.Set<OrderProduct>().AddAsync(orderProduct);
            }

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Order created with ID: {OrderId}", order.Id);
        }

        public async Task UpdateOrder(Order order, List<OrderProduct> orderProducts)
        {
            _dbContext.Set<Order>().Update(order);

            var existingOrderProducts = _dbContext.Set<OrderProduct>().Where(op => op.OrderId == order.Id);
            _dbContext.Set<OrderProduct>().RemoveRange(existingOrderProducts);

            foreach (var orderProduct in orderProducts)
            {
                orderProduct.OrderId = order.Id;
                await _dbContext.Set<OrderProduct>().AddAsync(orderProduct);
            }

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Order updated with ID: {OrderId}", order.Id);
        }

        public async Task DeleteOrder(int orderId)
        {
            var order = await _dbContext.Set<Order>().FindAsync(orderId);
            if (order != null)
            {
                var orderProducts = _dbContext.Set<OrderProduct>().Where(op => op.OrderId == orderId);
                _dbContext.Set<OrderProduct>().RemoveRange(orderProducts);
                _dbContext.Set<Order>().Remove(order);

                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Order deleted with ID: {OrderId}", orderId);
            }
        }
    }

}

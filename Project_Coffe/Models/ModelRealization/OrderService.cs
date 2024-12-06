using Project_Coffe.Data;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;

namespace Project_Coffe.Models.ModelRealization
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

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
            _logger.LogInformation("Fetching all orders");
            List<Order> orders = await _dbContext.Set<Order>()
                                         .Include(o => o.OrderProducts)
                                         .ThenInclude(op => op.Product)
                                         .ToListAsync();
            _logger.LogInformation($"Total orders fetched: {orders.Count}");
            return orders;
        }

        public async Task<Order?> GetOrderById(int orderId)
        {
            try
            {
                _logger.LogInformation($"Fetching order with ID: {orderId}");
                Order? order = await _dbContext.Set<Order>()
                                             .Include(o => o.OrderProducts)
                                             .ThenInclude(op => op.Product)
                                             .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    _logger.LogWarning($"Order with ID {orderId} not found.");
                }
                else
                {
                    _logger.LogInformation($"Order with ID {orderId} found.");
                }

                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching order with ID {orderId}: {ex.Message}");
                throw;
            }
        }

        public async Task CreateOrder(Order order, List<OrderProduct> orderProducts)
        {
            try
            {
                _logger.LogInformation($"Creating order for user with ID: {order.UserId}");

                await _dbContext.Set<Order>().AddAsync(order);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Order created with ID: {order.UserId}");

                decimal totalAmount = 0;

                foreach (OrderProduct orderProduct in orderProducts)
                {
                    _logger.LogInformation($"Processing product with ID: {orderProduct.ProductId}");

                    orderProduct.OrderId = order.Id;

                    Product? product = await _dbContext.Set<Product>().FindAsync(orderProduct.ProductId);
                    if (product == null)
                    {
                        _logger.LogError($"Product with ID {orderProduct.ProductId} not found.");
                        throw new Exception($"Product with ID {orderProduct.ProductId} not found.");
                    }

                    orderProduct.Order = order;
                    orderProduct.Product = product;
                    orderProduct.Subtotal = product.Price * orderProduct.Quantity;

                    _logger.LogInformation($"Product found: {orderProduct.Product.Name}, Quantity: {orderProduct.Quantity}, Subtotal: {orderProduct.Subtotal}");

                    totalAmount += orderProduct.Subtotal;

                    await _dbContext.Set<OrderProduct>().AddAsync(orderProduct);
                }

                order.TotalAmount = totalAmount;
                _logger.LogInformation($"Total amount for order ID {order.Id}: {totalAmount}");

                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Order with ID: {order.Id} successfully saved to the database.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating order: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateOrder(Order order, List<OrderProduct> orderProducts)
        {
            try
            {
                _logger.LogInformation($"Updating order for user with ID: {order.UserId}");

                _dbContext.Set<Order>().Update(order);

                List<OrderProduct> existingOrderProducts = await _dbContext.Set<OrderProduct>().Where(op => op.OrderId == order.Id).ToListAsync();
                _dbContext.Set<OrderProduct>().RemoveRange(existingOrderProducts);

                foreach (OrderProduct orderProduct in orderProducts)
                {
                    orderProduct.OrderId = order.Id;
                    await _dbContext.Set<OrderProduct>().AddAsync(orderProduct);
                }

                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Order updated with ID: {order.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating order with ID {order.Id}: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteOrder(int orderId)
        {
            try
            {
                Order? order = await _dbContext.Set<Order>().FindAsync(orderId);
                if (order != null)
                {
                    var orderProducts = _dbContext.Set<OrderProduct>().Where(op => op.OrderId == orderId);
                    _dbContext.Set<OrderProduct>().RemoveRange(orderProducts);
                    _dbContext.Set<Order>().Remove(order);

                    await _dbContext.SaveChangesAsync();
                    _logger.LogInformation($"Order deleted with ID: {orderId}");
                }
                else
                {
                    _logger.LogWarning($"Order with ID {orderId} not found for deletion.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting order with ID {orderId}: {ex.Message}");
                throw;
            }
        }
    }
}

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

        public async Task<Order> CreateOrUpdateOrder(int userId, List<OrderProduct> orderProducts)
        {
            Order? activeOrder = await GetActiveOrder(userId);

            if (activeOrder != null)
            {
                await AddProductsToActiveOrder(activeOrder.Id, orderProducts);
                return activeOrder;
            }
            else
            {
                return await CreateOrder(userId, orderProducts);
            }
        }

        public async Task<Order?> GetActiveOrder(int userId)
        {
            return await _dbContext.Set<Order>()
                .Include(o => o.User)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.IsActive);
        }


        public async Task AddProductsToActiveOrder(int orderId, List<OrderProduct> orderProducts)
        {
            Order? order = await _dbContext.Set<Order>()
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            decimal additionalAmount = 0;

            foreach (OrderProduct product in orderProducts)
            {
                Product? existingProduct = await _dbContext.Set<Product>().FindAsync(product.ProductId);

                if (existingProduct == null)
                {
                    throw new Exception($"Product with ID {product.ProductId} not found");
                }

                if (existingProduct.Stock < product.Quantity)
                {
                    _logger.LogError($"Not enough stock for product with ID {product.ProductId}. Available: {existingProduct.Stock}, Requested: {product.Quantity}");
                    throw new Exception($"Not enough stock for product with ID {product.ProductId}. Available: {existingProduct.Stock}, Requested: {product.Quantity}");
                }
                product.Product = existingProduct;
                product.OrderId = order.Id;
                product.CalculateSubtotal();
                existingProduct.Stock -= product.Quantity;
                additionalAmount += product.Subtotal;

                order.OrderProducts.Add(product);
            }

            order.TotalAmount += additionalAmount;

            _dbContext.Set<Order>().Update(order);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<Order> CreateOrder(int userId, List<OrderProduct> orderProducts)
        {
            Order order = new Order
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                OrderProducts = new List<OrderProduct>()
            };

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation($"Creating order for user with ID: {order.UserId}");

                await _dbContext.Set<Order>().AddAsync(order);
                await _dbContext.SaveChangesAsync();

                int totalAmountKopek = 0;

                foreach (OrderProduct orderProduct in orderProducts)
                {
                    Product? existingProduct = await _dbContext.Set<Product>()
                                                   .FirstOrDefaultAsync(p => p.Id == orderProduct.ProductId);

                    if (existingProduct == null)
                    {
                        _logger.LogError($"Product with ID {orderProduct.ProductId} not found.");
                        throw new Exception($"Product with ID {orderProduct.ProductId} not found.");
                    }
                    if (existingProduct.Stock < orderProduct.Quantity)
                    {
                        _logger.LogError($"Not enough stock for product with ID {orderProduct.ProductId}. Available: {existingProduct.Stock}, Requested: {orderProduct.Quantity}");
                        throw new Exception($"Not enough stock for product with ID {orderProduct.ProductId}. Available: {existingProduct.Stock}, Requested: {orderProduct.Quantity}");
                    }

                    orderProduct.OrderId = order.Id;
                    orderProduct.Product = existingProduct;
                    orderProduct.CalculateSubtotal();
                    existingProduct.Stock -= orderProduct.Quantity;
                    totalAmountKopek += ConvertDecimalToInt(orderProduct.Subtotal);

                    order.OrderProducts.Add(orderProduct);
                }

                order.TotalAmount = ConvertIntToDecimal(totalAmountKopek); ;
                await _dbContext.Set<OrderProduct>().AddRangeAsync(orderProducts);
                await _dbContext.SaveChangesAsync();

                order.User = await _dbContext.Set<User>().FindAsync(userId);

                await transaction.CommitAsync();

                _logger.LogInformation($"Order with ID: {order.Id} successfully saved to the database.");
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating order: {ex.Message}");
                await transaction.RollbackAsync();
                throw;
            }
        }

        private int ConvertDecimalToInt(decimal value)
        {
            return (int)Math.Round(value * 100);
        }
        private decimal ConvertIntToDecimal(int value)
        {
            return value / 100m;
        }

        public async Task UpdateOrder(Order order)
        {
            try
            {
                _logger.LogInformation($"Updating order for user with ID: {order.UserId}");

                _dbContext.Set<Order>().Update(order);

                List<OrderProduct> existingOrderProducts = await _dbContext.Set<OrderProduct>()
                    .Where(op => op.OrderId == order.Id)
                    .ToListAsync();
                _dbContext.Set<OrderProduct>().RemoveRange(existingOrderProducts);

                foreach (OrderProduct orderProduct in order.OrderProducts)
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
                    List<OrderProduct> orderProducts = await _dbContext.Set<OrderProduct>()
                                                         .Where(op => op.OrderId == orderId)
                                                         .ToListAsync();

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

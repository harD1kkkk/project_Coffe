using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Coffe.DTO;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;

namespace Project_Coffe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("get-all-orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                List<Order> orders = (await _orderService.GetAllOrders()).ToList();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching orders: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-order-by-id/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                Order? order = await _orderService.GetOrderById(id);
                if (order == null)
                {
                    _logger.LogWarning($"Order with ID {id} not found.");
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching order with ID {id}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO createOrderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for CreateOrder request.");
                    return BadRequest(ModelState);
                }

                int userId = createOrderDto.UserId;
                List<CreateOrderProductDTO>? orderProductsDto = createOrderDto.OrderProducts;

                if (orderProductsDto == null || !orderProductsDto.Any())
                {
                    _logger.LogError("Order products list is null or empty.");
                    return BadRequest("Order products cannot be null or empty.");
                }

                List<OrderProduct> orderProducts = orderProductsDto.Select(dto => new OrderProduct
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                }).ToList();

                Order order = await _orderService.CreateOrUpdateOrder(userId, orderProducts);

                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating or updating order: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpPut("update-order/{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDTO updateOrderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning($"Invalid request data for updating order with ID {id}.");
                    return BadRequest(ModelState);
                }

                Order? order = await _orderService.GetOrderById(id);
                if (order == null)
                {
                    _logger.LogWarning($"Order with ID {id} not found.");
                    return NotFound();
                }

                order.UserId = updateOrderDto.UserId;
                order.IsActive = updateOrderDto.IsActive;
                order.TotalAmount = updateOrderDto.TotalAmount;

                List<OrderProduct> orderProducts = updateOrderDto.OrderProducts.Select(op => new OrderProduct
                {
                    OrderId = id,
                    ProductId = op.ProductId,
                    Quantity = op.Quantity,
                    Subtotal = op.Subtotal
                }).ToList();

                order.OrderProducts = orderProducts;

                await _orderService.UpdateOrder(order);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating order with ID {id}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-order/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.DeleteOrder(id);
                _logger.LogInformation($"Order with ID {id} successfully deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting order with ID {id}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

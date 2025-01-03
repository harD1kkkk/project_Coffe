<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Coffe.DTO;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;
=======
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;
using Project_Coffe.Models.ModelRealization;
using System.Security.Claims;
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9

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

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
<<<<<<< HEAD
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
=======
            var orders = await _orderService.GetAllOrders();
            return Ok(orders);
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
<<<<<<< HEAD
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

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO createOrderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for CreateOrder request.");
                    return BadRequest(ModelState);
                }

                var userId = createOrderDto.UserId;
                var orderProductsDto = createOrderDto.OrderProducts;

                if (orderProductsDto == null || !orderProductsDto.Any())
                {
                    _logger.LogError("Order products list is null or empty.");
                    return BadRequest("Order products cannot be null or empty.");
                }

                var orderProducts = orderProductsDto.Select(dto => new OrderProduct
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                }).ToList();

                var order = await _orderService.CreateOrUpdateOrder(userId, orderProducts);

                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating or updating order: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDTO updateOrderDto)
        {
            try
            {
                if (updateOrderDto.Order == null)
                {
                    _logger.LogWarning("UpdateOrder: Order is null.");
                    return BadRequest("Order cannot be null.");
                }

                if (!ModelState.IsValid || id != updateOrderDto.Order.Id)
                {
                    _logger.LogWarning($"Invalid request data for updating order with ID {id}.");
                    return BadRequest(ModelState);
                }

                if (updateOrderDto.OrderProducts == null)
                {
                    _logger.LogWarning($"Order products list is null for order with ID {id}.");
                    return BadRequest("Order products cannot be null.");
                }

                await _orderService.UpdateOrder(updateOrderDto.Order, updateOrderDto.OrderProducts);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating order with ID {id}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
=======
            var order = await _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order, [FromBody] List<OrderProduct> orderProducts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _orderService.CreateOrder(order, orderProducts);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order, [FromBody] List<OrderProduct> orderProducts)
        {
            if (!ModelState.IsValid || id != order.Id)
            {
                return BadRequest(ModelState);
            }

            await _orderService.UpdateOrder(order, orderProducts);
            return NoContent();
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
<<<<<<< HEAD
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
=======
            await _orderService.DeleteOrder(id);
            return NoContent();
        }
    }

>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
}

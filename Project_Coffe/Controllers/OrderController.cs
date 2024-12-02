using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Coffe.DTO;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;
using Project_Coffe.Models.ModelRealization;
using System.Security.Claims;

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
            var orders = await _orderService.GetAllOrders();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO createOrderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = createOrderDto.Order;
            var orderProducts = createOrderDto.OrderProducts;

            await _orderService.CreateOrder(order, orderProducts);

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDTO updateOrderDto)
        {
            if (!ModelState.IsValid || id != updateOrderDto.Order.Id)
            {
                return BadRequest(ModelState);
            }

            await _orderService.UpdateOrder(updateOrderDto.Order, updateOrderDto.OrderProducts);

            return NoContent(); 
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrder(id);
            return NoContent();
        }
    }

}

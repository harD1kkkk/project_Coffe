using Microsoft.AspNetCore.Mvc;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;

namespace Project_Coffe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string name, string email, string password)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Name, email, and password are required.");
            }
            var user = await _userService.Register(name, email, password);
            if (user == null)
            {
                return BadRequest("Email is already taken.");
            }

            return Ok(new { user.Id, user.Name, user.Email });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var token = await _userService.Login(email, password);
            if (token == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(new { Token = token });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new { user.Id, user.Name, user.Email });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, string name, string email, string password)
        {
            var updatedUser = await _userService.UpdateUser(id, name, email, password);
            if (updatedUser == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new { updatedUser.Id, updatedUser.Name, updatedUser.Email });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);
            if (!result)
            {
                return NotFound("User not found.");
            }

            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;
using Project_Coffe.Models.ModelRealization;
namespace Project_Coffe.Controllers
{
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
                var user = await _userService.Register(name, email, password);
                if (user == null)
                    return BadRequest("Email is already taken.");

                return Ok(new { user.Id, user.Name, user.Email });
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login(string email, string password)
            {
                var user = await _userService.Login(email, password);
                if (user == null)
                    return Unauthorized("Invalid email or password.");

                return Ok(new { user.Id, user.Name, user.Email });
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetUserById(int id)
            {
                var user = await _userService.GetUserById(id);
                if (user == null)
                    return NotFound("User not found.");

                return Ok(new { user.Id, user.Name, user.Email });
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateUser(int id, string name, string email, string password)
            {
                var updatedUser = await _userService.UpdateUser(id, name, email, password);
                if (updatedUser == null)
                    return NotFound("User not found.");

                return Ok(new { updatedUser.Id, updatedUser.Name, updatedUser.Email });
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteUser(int id)
            {
                var result = await _userService.DeleteUser(id);
                if (!result)
                    return NotFound("User not found.");

                return NoContent();
            }
        }
    }
}


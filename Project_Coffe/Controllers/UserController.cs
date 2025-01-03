using Microsoft.AspNetCore.Mvc;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;
<<<<<<< HEAD

namespace Project_Coffe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string name, string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    _logger.LogWarning("Registration failed: Missing name, email, or password.");
                    return BadRequest("Name, email, and password are required.");
                }

                User? user = await _userService.Register(name, email, password);
                if (user == null)
                {
                    _logger.LogWarning($"Registration failed: Email {email} is already taken.");
                    return BadRequest("Email is already taken.");
                }

                _logger.LogInformation($"User {user.Name} registered successfully with email {user.Email}.");
                return Ok(new { user.Id, user.Name, user.Email });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during registration: {ex.Message}");
                return StatusCode(500, "An error occurred during registration.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                string? token = await _userService.Login(email, password);
                if (token == null)
                {
                    _logger.LogWarning($"Login failed: Invalid email or password for {email}.");
                    return Unauthorized("Invalid email or password.");
                }

                _logger.LogInformation($"User {email} logged in successfully.");
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during login: {ex.Message}");
                return StatusCode(500, "An error occurred during login.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                User? user = await _userService.GetUserById(id);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                    return NotFound("User not found.");
                }

                return Ok(new { user.Id, user.Name, user.Email });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching user with ID {id}: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching the user.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, string name, string email, string password)
        {
            try
            {
                User? updatedUser = await _userService.UpdateUser(id, name, email, password);
                if (updatedUser == null)
                {
                    _logger.LogWarning($"Update failed: User with ID {id} not found.");
                    return NotFound("User not found.");
                }

                _logger.LogInformation($"User with ID {id} updated successfully.");
                return Ok(new { updatedUser.Id, updatedUser.Name, updatedUser.Email });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user with ID {id}: {ex.Message}");
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                bool result = await _userService.DeleteUser(id);
                if (!result)
                {
                    _logger.LogWarning($"Delete failed: User with ID {id} not found.");
                    return NotFound("User not found.");
                }

                _logger.LogInformation($"User with ID {id} deleted successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting user with ID {id}: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the user.");
            }
        }
    }
}
=======
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

>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9

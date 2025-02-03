using Microsoft.AspNetCore.Mvc;
using Project_Coffe.DTO;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;

namespace Project_Coffe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoffeeRecommendationController : ControllerBase
    {
        private readonly ICoffeeRecommendationService _coffeeRecommendation;
        private readonly ILogger<CoffeeRecommendationController> _logger;

        public CoffeeRecommendationController(ICoffeeRecommendationService coffeeRecommendation, ILogger<CoffeeRecommendationController> logger)
        {
            _coffeeRecommendation = coffeeRecommendation;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> RecommendCoffee([FromBody] CreateUserPreferencesDTO preferences)
        {
            try
            {
                if (preferences == null)
                {
                    return BadRequest("User preferences should not be empty.");
                }
                if (string.IsNullOrEmpty(preferences.Flavor) || string.IsNullOrEmpty(preferences.Intensity) || string.IsNullOrEmpty(preferences.MilkPreference))
                {
                    _logger.LogError("Flavor or Intensity or MilkPreference is Null");
                    return BadRequest("Flavor or Intensity or MilkPreference is Null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Product creation failed: Invalid model state.");
                    return BadRequest(ModelState);
                }
                UserPreference userpreferences = new UserPreference
                {
                    Id = 0,
                    UserId = preferences.UserId,
                    Flavor = preferences.Flavor,
                    Intensity = preferences.Intensity,
                    MilkPreference = preferences.MilkPreference,
                    ResponseFromGPT = "",
                    Response = true,
                    ResponseFromMircoService = false
                };
                string response = await _coffeeRecommendation.SendToMicroServiceCoffeeRecommendation(userpreferences);
                _logger.LogInformation("Coffee recommendation sent successfully.");
                if (response == null)
                {
                    return StatusCode(500, "Internal server error");
                }
                return Ok(response);

            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

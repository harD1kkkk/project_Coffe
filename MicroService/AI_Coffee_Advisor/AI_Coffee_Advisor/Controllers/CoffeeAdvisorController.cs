//using AI_Coffee_Advisor.Entities;
//using AI_Coffee_Advisor.Model.ModelInterface;
//using Microsoft.AspNetCore.Mvc;
//using System.Text.Json;

//namespace AI_Coffee_Advisor.Controllers
//{
//    //[ApiController]
//    //[Route("api/[controller]")]
//    //public class CoffeeRecommendationController : ControllerBase
//    //{
//    //    private readonly ICoffeeRecommendation _coffeeRecommendation;

//    //    public CoffeeRecommendationController(ICoffeeRecommendation coffeeRecommendation)
//    //    {
//    //        _coffeeRecommendation = coffeeRecommendation;
//    //    }

//    //    [HttpPost("recommend-coffee")]
//    //    public async Task<IActionResult> RecommendCoffee([FromBody] UserPreference preferences)
//    //    {
//    //        if (preferences == null || string.IsNullOrEmpty(preferences.ToDescription()))
//    //        {
//    //            return BadRequest("User preferences should not be empty.");
//    //        }

//    //        try
//    //        {
//    //            string responseContent = await _coffeeRecommendation.GetCoffeeRecommendation(preferences);
//    //            return Ok(responseContent);
//    //        }
//    //        catch (HttpRequestException ex)
//    //        {
//    //            return StatusCode(500, ex.Message);
//    //        }
//    //    }
//    //}
//}

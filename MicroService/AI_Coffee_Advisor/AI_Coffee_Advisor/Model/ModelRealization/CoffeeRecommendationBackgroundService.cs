using AI_Coffee_Advisor.Model.ModelInterface;

namespace AI_Coffee_Advisor.Model.ModelRealization
{
    public class CoffeeRecommendationBackgroundService : BackgroundService
    {
        private readonly ICoffeeRecommendation _coffeeRecommendation;
        private readonly ILogger<CoffeeRecommendationBackgroundService> _logger;

        public CoffeeRecommendationBackgroundService(
            ICoffeeRecommendation coffeeRecommendation,
            ILogger<CoffeeRecommendationBackgroundService> logger)
        {
            _coffeeRecommendation = coffeeRecommendation;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CoffeeRecommendationBackgroundService is starting.");

            try
            {
                await _coffeeRecommendation.GetCoffeeRecommendation();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in background service: {ex.Message}");
            }
        }
    }

}

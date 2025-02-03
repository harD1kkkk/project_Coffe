using Microsoft.EntityFrameworkCore;
using Project_Coffe.Data;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;

namespace Project_Coffe.Models.ModelRealization
{
    public class CoffeeRecommendationService : ICoffeeRecommendationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CoffeeRecommendationService> _logger;

        public CoffeeRecommendationService(ApplicationDbContext dbContext, ILogger<CoffeeRecommendationService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<string?> SendToMicroServiceCoffeeRecommendation(UserPreference preferences)
        {
            try
            {
                if (preferences == null)
                {
                    _logger.LogWarning("preferences null");
                    throw new ArgumentNullException(nameof(preferences));
                }
                preferences.Response = true;
                await _dbContext.Set<UserPreference>().AddAsync(preferences);
                await _dbContext.SaveChangesAsync();
                string? response = "";

                while (true)
                {
                    UserPreference? userPreference = await _dbContext.UserPreferences.FirstOrDefaultAsync(up => up.UserId == preferences.UserId);
                    if (userPreference == null)
                    {
                        _logger.LogWarning($"preferences with ID {preferences.Id} not found.");
                        break;
                    }
                    if (userPreference.ResponseFromMircoService == false)
                    {
                        await Task.Delay(1000);
                    }
                    if (userPreference.ResponseFromMircoService)
                    {
                        response = userPreference.ResponseFromGPT;
                        _dbContext.Set<UserPreference>().Remove(userPreference);
                        await _dbContext.SaveChangesAsync();
                        _logger.LogInformation($"User Preference deleted with ID: {userPreference.Id}");
                        break;
                    }
                }
                return preferences.ResponseFromGPT = response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending to microservice: {ex.Message}");
                throw new Exception("An error occurred while sending to microservice.");
            }
        }
    }
}


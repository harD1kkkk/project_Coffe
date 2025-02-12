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
                User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == preferences.UserId);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {preferences.UserId} not found.");
                    return null;
                }
                preferences.Response = true;
                await _dbContext.Set<UserPreference>().AddAsync(preferences);
                await _dbContext.SaveChangesAsync();
                int preferenceId = preferences.Id;
                string? response = "";
                int count = 0;

                while (true)
                {
                    UserPreference? currentPreference = await _dbContext.UserPreferences
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(up => up.Id == preferenceId);
                    if (currentPreference == null)
                    {
                        _logger.LogWarning($"preferences with ID {preferenceId} not found.");
                        break;
                    }

                    if (currentPreference.ResponseFromMicroService == false)
                    {
                        if (count > 10)
                        {
                            _logger.LogWarning($"Something wrong with micro service.");
                            break;
                        }
                        count++;
                        await Task.Delay(1000);
                    }
                    if (currentPreference.ResponseFromMicroService)
                    {
                        response = currentPreference.ResponseFromGPT;
                        UserPreference? preferenceToDelete = await _dbContext.UserPreferences.FindAsync(preferenceId);
                        if (preferenceToDelete != null)
                        {
                            _dbContext.UserPreferences.Remove(preferenceToDelete);
                            await _dbContext.SaveChangesAsync();
                            _logger.LogInformation($"User Preference deleted with ID: {currentPreference.Id}");
                        }
                        break;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending to microservice: {ex.Message}");
                throw new Exception("An error occurred while sending to microservice.", ex);
            }
        }
    }
}


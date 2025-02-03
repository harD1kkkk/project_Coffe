using AI_Coffee_Advisor.Data;
using AI_Coffee_Advisor.Entities;
using AI_Coffee_Advisor.Model.ModelInterface;
using Microsoft.EntityFrameworkCore;

namespace AI_Coffee_Advisor.Model.ModelRealization
{
    public class CoffeeRecommendationService : ICoffeeRecommendation
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        private readonly HttpClient _httpClient;
        private readonly ILogger<CoffeeRecommendationService> _logger;

        public CoffeeRecommendationService(IDbContextFactory<ApplicationDbContext> dbContextFactory, HttpClient httpClient, ILogger<CoffeeRecommendationService> logger)
        {
            _dbContextFactory = dbContextFactory;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GetCoffeeRecommendation()
        {
            while (true)
            {
                using var dbContext = _dbContextFactory.CreateDbContext();
                List<UserPreference> userPreferences = await dbContext.Set<UserPreference>()
                    .Where(p => p.Response && !p.ResponseFromMircoService)
                    .ToListAsync();

                if (userPreferences.Count == 0)
                {
                    await Task.Delay(1000);
                    continue;
                }

                foreach (UserPreference preference in userPreferences)
                {
                    try
                    {
                        string preferencesDescription = preference.ToDescription();
                        string query = $"Recommend coffee with the following descriptions: {preferencesDescription}";
                        string apiUrl = $"https://free-unoficial-gpt4o-mini-api-g70n.onrender.com/chat/?query={query}";

                        var response = await _httpClient.GetAsync(apiUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            string responseContent = await response.Content.ReadAsStringAsync();
                            preference.ResponseFromGPT = responseContent;
                            preference.ResponseFromMircoService = true;
                            await dbContext.SaveChangesAsync();
                        }
                        else
                        {
                            _logger.LogError($"Request to API failed with status code {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Exception occurred while processing preferences: {ex.Message}");
                    }
                }
                await Task.Delay(1000);
            }
        }

    }
}


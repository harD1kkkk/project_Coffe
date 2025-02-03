using Project_Coffe.Entities;

namespace Project_Coffe.Models.ModelInterface
{
    public interface ICoffeeRecommendationService
    {
        Task<string> SendToMicroServiceCoffeeRecommendation(UserPreference preferences);
    }
}

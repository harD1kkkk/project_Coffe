using AI_Coffee_Advisor.Entities;

namespace AI_Coffee_Advisor.Model.ModelInterface
{
    public interface ICoffeeRecommendation
    {
        Task<string> GetCoffeeRecommendation();
    }
}

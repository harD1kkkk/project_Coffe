namespace AI_Coffee_Advisor.Entities
{
    public class UserPreference
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Flavor { get; set; }
        public required string Intensity { get; set; }
        public required string MilkPreference { get; set; }
        public string? ResponseFromGPT { get; set; }
        public bool Response { get; set; } = true;
        public bool ResponseFromMircoService { get; set; } = false;

        public string ToDescription()
        {
            return $"Taste: {Flavor}, Intensity: {Intensity}, Milk Preference: {MilkPreference}";
        }
    }
}

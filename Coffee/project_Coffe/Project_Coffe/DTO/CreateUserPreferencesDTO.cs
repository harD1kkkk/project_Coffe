using System.ComponentModel.DataAnnotations;

namespace Project_Coffe.DTO
{
    public class CreateUserPreferencesDTO
    {
        [Required]
        public int UserId { get; set; }
        public string? Flavor { get; set; }
        public string? Intensity { get; set; }
        public string? MilkPreference { get; set; }
    }
}

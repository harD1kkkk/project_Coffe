using System.ComponentModel.DataAnnotations;

namespace Project_Coffe.Entities
{
    public class UserPreference
    {
        [Required]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Flavor { get; set; }
        public string? Intensity { get; set; }
        public string? MilkPreference { get; set; }
        public string? ResponseFromGPT { get; set; }
        public bool Response { get; set; } = true;
        public bool ResponseFromMircoService { get; set; } = false;
    }
}

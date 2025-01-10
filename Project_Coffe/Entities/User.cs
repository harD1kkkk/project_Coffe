using System.ComponentModel.DataAnnotations;

namespace Project_Coffe.Entities
{
    public class User
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name length can't be more than 100.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "PasswordHash is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password length must be between 6 and 100 characters.")]
        public string? PasswordHash { get; set; }
    }
}

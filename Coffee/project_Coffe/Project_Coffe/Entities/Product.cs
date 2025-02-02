using System.ComponentModel.DataAnnotations;

namespace Project_Coffe.Entities
{
    public class Product
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name length can't be more than 100.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10000.00")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Description length can't be more than 500.")]
        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a positive number")]
        public int Stock { get; set; }

        public string? ImagePath { get; set; }

        public string? MusicPath { get; set; }
    }
}

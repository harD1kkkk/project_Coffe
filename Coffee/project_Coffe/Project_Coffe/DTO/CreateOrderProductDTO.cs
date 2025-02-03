using System.ComponentModel.DataAnnotations;

namespace Project_Coffe.DTO
{
    public class CreateOrderProductDTO
    {
        [Required]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}

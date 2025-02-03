using System.ComponentModel.DataAnnotations;

namespace Project_Coffe.DTO
{
    public class UpdateOrderProductDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Subtotal must be positive")]
        public decimal Subtotal { get; set; }
    }
}
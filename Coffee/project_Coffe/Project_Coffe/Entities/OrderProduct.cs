using System.ComponentModel.DataAnnotations;

namespace Project_Coffe.Entities
{
    public class OrderProduct
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "OrderId is required")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "ProductId is required")]
        public int ProductId { get; set; }

        [Required]
        public Product? Product { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Subtotal is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Subtotal must be positive")]
        public decimal Subtotal { get; set; }

        public void CalculateSubtotal()
        {
            if (Product != null)
            {
                Subtotal = Product.Price * Quantity;
            }
        }
    }
}

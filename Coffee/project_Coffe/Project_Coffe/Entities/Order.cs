using System.ComponentModel.DataAnnotations;

namespace Project_Coffe.Entities
{
    public class Order
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }

        public User? User { get; set; }

        [Required(ErrorMessage = "CreatedAt is required")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        [Required]
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

        [Required(ErrorMessage = "TotalAmount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "TotalAmount must be positive")]
        public decimal TotalAmount { get; set; }

        //public void CalculateTotalAmount()
        //{
        //    TotalAmount = OrderProducts.Sum(op => op.Subtotal);
        //}
    }
}

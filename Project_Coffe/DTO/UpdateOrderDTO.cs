using Project_Coffe.Entities;
using System.ComponentModel.DataAnnotations;

namespace Project_Coffe.DTO
{
    public class UpdateOrderDTO
    {
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public ICollection<UpdateOrderProductDTO> OrderProducts { get; set; } = new List<UpdateOrderProductDTO>();

        [Required(ErrorMessage = "TotalAmount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "TotalAmount must be positive")]
        public decimal TotalAmount { get; set; }
    }

}

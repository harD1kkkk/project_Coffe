using Project_Coffe.Entities;
using System.ComponentModel.DataAnnotations;

namespace Project_Coffe.DTO
{
    public class CreateOrderDTO
    {
        [Required]
        public int UserId { get; set; }
        public List<CreateOrderProductDTO>? OrderProducts { get; set; } 
    }
}


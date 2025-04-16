using System.ComponentModel.DataAnnotations;

namespace Project_Coffe.DTO
{
    public class CreateOrderProductDTO
    {
        public int ProductId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public int Quantity { get; set; }
    }
}

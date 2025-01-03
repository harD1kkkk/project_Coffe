<<<<<<< HEAD
﻿namespace Project_Coffe.Entities
=======
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Coffe.Entities
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
<<<<<<< HEAD
        public bool IsActive { get; set; } = true;
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
        public decimal TotalAmount { get; set; }

        public void CalculateTotalAmount()
        {
            TotalAmount = OrderProducts.Sum(op => op.Subtotal);
        }
=======

        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
        public decimal TotalAmount { get; set; }
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
    }
}

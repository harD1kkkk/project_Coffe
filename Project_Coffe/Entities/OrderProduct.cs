namespace Project_Coffe.Entities
{
    public class OrderProduct
    {
        public int Id { get; set; }
<<<<<<< HEAD
        public int OrderId { get; set; }  
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }

        public void CalculateSubtotal()
        {
            if (Product != null)
            {
                Subtotal = Product.Price * Quantity;
            }
        }
=======
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
>>>>>>> 0d50e16b2a6a77a4377ebb9f8c716686a9238ed9
    }
}

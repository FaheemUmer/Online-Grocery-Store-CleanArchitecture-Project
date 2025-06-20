namespace Web.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; } // Foreign key to Order
        public int ProductId { get; set; } // Foreign key to Product
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}

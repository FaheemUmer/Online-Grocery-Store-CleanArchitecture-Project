namespace Domain.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; } // Foreign key to User
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // e.g., Pending, Shipped, Delivered

		public User User { get; set; }  // Navigation to User
		public ICollection<OrderDetail> OrderDetails { get; set; } // Navigation property

		public Payment Payment { get; set; } // Navigation to Payment
	}
}

namespace Domain.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; } // Foreign key to Order
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } // e.g., Credit Card, Cash on Delivery
        public decimal Amount { get; set; }
        public string Status { get; set; } // e.g., Paid, Failed, Pending

		// Navigation Property
		public Order Order { get; set; } // Navigation to Order
	}
}

namespace Web.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int UserId { get; set; } // Foreign key to User
        public int ProductId { get; set; } // Foreign key to Product
        public int Quantity { get; set; }

		public User User { get; set; } // Navigation to User
		public Product Product { get; set; } // Navigation to Product
	}
}

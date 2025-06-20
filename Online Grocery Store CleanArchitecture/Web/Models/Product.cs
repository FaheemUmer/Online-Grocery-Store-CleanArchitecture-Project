namespace Web.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }
        public int Price { get; set; }

        public string? ImageUrl { get; set; } // Allow null if the image is uploaded separately

        public int Quantity { get; set; }

		// Navigation Properties
		public Category Category { get; set; } // Navigation to Category
		public ICollection<OrderDetail> OrderDetails { get; set; } // Navigation to OrderDetails
		public ICollection<CartItem> CartItems { get; set; } // Navigation to CartItems
		public ICollection<Review> Reviews { get; set; } // Navigation to Reviews
	}
}

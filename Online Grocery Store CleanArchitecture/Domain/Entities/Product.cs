using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public string CategoryId { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public int Price { get; set; }

        public string? ImageUrl { get; set; } // Allow null if the image is uploaded separately

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        // Navigation Properties
        public string Category { get; set; } // Navigation to Category
        public ICollection<OrderDetail> OrderDetails { get; set; } // Navigation to OrderDetails
        public ICollection<CartItem> CartItems { get; set; } // Navigation to CartItems
        public ICollection<Review> Reviews { get; set; } // Navigation to Reviews
    }
}

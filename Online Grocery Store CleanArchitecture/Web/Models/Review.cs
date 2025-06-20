namespace Web.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; } // Foreign key to Product
        public int UserId { get; set; } // Foreign key to User
        public string ReviewText { get; set; }
        public int Rating { get; set; } // e.g., 1 to 5 stars
        public DateTime ReviewDate { get; set; }
    }
}

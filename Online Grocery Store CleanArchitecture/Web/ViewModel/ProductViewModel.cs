namespace Web.ViewModel
{
    public class ProductViewModel
    {
        public int Id { get; set; }  // Optional if editing existing product

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public int Price { get; set; }

        public string? ImageUrl { get; set; }

        public int Quantity { get; set; }
    }
}

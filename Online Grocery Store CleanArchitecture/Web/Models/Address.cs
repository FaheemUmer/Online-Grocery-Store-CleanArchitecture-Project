namespace Web.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public int UserId { get; set; } // Foreign key to User
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public bool IsDefault { get; set; } // To mark as default address
    }
}

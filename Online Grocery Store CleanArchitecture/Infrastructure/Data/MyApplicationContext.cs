using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class MyApplicationContext:DbContext
    {
        public MyApplicationContext() { }

        public MyApplicationContext(DbContextOptions<MyApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Use the connection string from appsettings.json or hardcode it here
                var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OGSCA;Trusted_Connection=True;MultipleActiveResultSets=true";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}

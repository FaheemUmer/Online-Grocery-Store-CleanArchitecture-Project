using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AdminService:IAdminService
    {
        private readonly MyApplicationContext _context;

        public AdminService(MyApplicationContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> GetDashboardStatsAsync()
        {
            var totalProducts = await _context.Products.CountAsync();
            var totalOrders = await _context.Orders.CountAsync();
            var totalUsers = await _context.Users.CountAsync();

            return new DashboardViewModel
            {
                TotalProducts = totalProducts,
                TotalOrders = totalOrders,
                TotalUsers = totalUsers
            };
        }

        public async Task<IQueryable<Product>> GetProductsAsync(string searchQuery = "")
        {
            var productsQuery = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(searchQuery));
            }

            return await Task.FromResult(productsQuery);
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category) // Ensure Category is loaded
                .FirstOrDefaultAsync(p => p.Id == id);
        }


        public async Task AddProductAsync(Product product, IFormFile productImage)
        {
            if (productImage != null && productImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productImage.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await productImage.CopyToAsync(stream);
                }

                product.ImageUrl = "/images/" + fileName;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Product not found!");
            }
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ToListAsync();
        }


        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}

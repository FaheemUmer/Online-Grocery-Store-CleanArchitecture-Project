using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class HomeService:IHomeService
    {
        private readonly MyApplicationContext _context;

        public HomeService(MyApplicationContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId.ToString() == userId);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> SearchProductsAsync(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return new List<Product>();
            }

            return await _context.Products
                .Where(p => p.Name.ToLower().Contains(query.ToLower()))
                .ToListAsync();
        }

    }
}

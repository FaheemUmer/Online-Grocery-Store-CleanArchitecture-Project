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
    public class AccountService:IAccountService
    {
        private readonly MyApplicationContext _context;

        public AccountService(MyApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> SignUpAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) ||
                string.IsNullOrWhiteSpace(user.Password) ||
                string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.UserType))
            {
                throw new ArgumentException("All fields are required.");
            }

            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                throw new InvalidOperationException("Email is already associated with another user.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid Username or Password");
            }

            return user;
        }

        public Task LogoutAsync()
        {
            // Logout logic (if any) can be added here
            return Task.CompletedTask;
        }

    }
}

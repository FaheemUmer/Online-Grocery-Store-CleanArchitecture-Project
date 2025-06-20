using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IHomeService
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByIdAsync(string userId);
        Task UpdateUserAsync(User user);
        Task<List<Product>> SearchProductsAsync(string query);
    }
}

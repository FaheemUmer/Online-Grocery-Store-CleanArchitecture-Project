using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAdminService
    {
        Task<DashboardViewModel> GetDashboardStatsAsync();
        Task<IQueryable<Product>> GetProductsAsync(string searchQuery = "");
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product, IFormFile productImage);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<List<Order>> GetOrdersAsync();
        Task<User> GetUserByUsernameAsync(string username);
    }
}

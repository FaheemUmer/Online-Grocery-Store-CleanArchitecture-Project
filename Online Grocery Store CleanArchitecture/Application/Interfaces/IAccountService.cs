using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<bool> SignUpAsync(User user);
        Task<User> LoginAsync(string username, string password);
        Task LogoutAsync();
    }
}

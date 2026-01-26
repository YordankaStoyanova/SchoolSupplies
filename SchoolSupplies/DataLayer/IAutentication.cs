using BusinessLayer.Enum;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DataLayer
{
    public interface IAutentication
    {
        Task<IdentityUser> CreateUserAsync(string name, string password, string email, Role role);
        Task<User> LogInUserAsync(string email, string password);
        Task<User> ReadUserAsync(string key);
        Task<IEnumerable<User>> ReadAllUsersAsync();
        Task DeleteUserByNameAsync(string name);
        Task<User> FindUserByNEmailAsync(string name);
    }
}

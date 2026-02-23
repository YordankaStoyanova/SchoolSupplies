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
    public interface IAuthentication
    {
        Task<User> CreateUserAsync(string name, string email, string password);
        Task<User> LoginAsync(string email, string password);
        Task<User> ReadUserAsync(string id);
        Task<IEnumerable<User>> ReadAllUsersAsync();
        Task DeleteUserByIdAsync(string id);
        Task<User> FindUserByEmailAsync(string email);
    }
}

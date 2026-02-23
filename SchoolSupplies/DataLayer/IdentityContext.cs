using BusinessLayer;
using BusinessLayer.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class IdentityContext
    {
        private readonly SchoolSuppliesDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<User> _roleManager;

        public IdentityContext(SchoolSuppliesDbContext context, UserManager<User> userManager, RoleManager<User> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<User> CreateUserAsync(string name, string password, string email, string role)
        {
            var user = new User
            {
                UserName = email,
                Email = email,
                Name = name
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                throw new ArgumentException(result.Errors.First().Description);

            if (!await _roleManager.RoleExistsAsync(role))
                throw new ArgumentException("Role does not exist");

            await _userManager.AddToRoleAsync(user, role);

            return user;
        }

        public async Task<User> LogInUserAsync(string email, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null) return null;

                var result = await _userManager.PasswordValidators[1].ValidateAsync(_userManager, user, password);

                if (result.Succeeded) return await _context.Users.FindAsync(user.Id);

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<User> ReadUserAsync(string key)
        {
            var query = _context.Users;
            var user = await query.FirstOrDefaultAsync(u => u.Id == key);
            return user;
        }

        public async Task<IEnumerable<User>> ReadAllUsersAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

         public async Task UpdateUserAsync(string id, string username, string name, int age)
         {
             try
             {
                 if (!string.IsNullOrEmpty(username))
                 {
                     User user = await _context.Users.FindAsync(id);
                   _context.Entry(user).CurrentValues.SetValues(user);
                     await _userManager.UpdateAsync(user);
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
    
        public async Task DeleteUserByNameAsync(string name)
        {
            try
            {
                var user = await FindUserByNEmailAsync(name);

                if (user == null) throw new InvalidOperationException("User not found for deletion!");

                await _userManager.DeleteAsync(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> FindUserByNEmailAsync(string name)
        {
            try
            {
                // Identity return Null if there is no user!
                return await _userManager.FindByEmailAsync(name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}



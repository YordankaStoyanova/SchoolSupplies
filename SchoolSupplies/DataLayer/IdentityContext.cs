using BusinessLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class IdentityContext
    {
        private readonly SchoolSuppliesDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityContext(SchoolSuppliesDbContext context, UserManager<User> userManager,RoleManager<IdentityRole> roleManager)
        {
            _dbContext = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<User> RegisterAsync(
       string email,
       string name,
       string password,
       UserRole role)
        {
            var user = new User(email, name);

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join("; ",
                    result.Errors.Select(e => e.Description)));
            }

           
            if (!await _roleManager.RoleExistsAsync(role.ToString()))
            {
                await _roleManager.CreateAsync(
                    new IdentityRole(role.ToString()));
            }

            await _userManager.AddToRoleAsync(user, role.ToString());

            return user;
        }

       
        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            var isValid = await _userManager.CheckPasswordAsync(user, password);
            return isValid ? user : null;
        }

       
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

       
        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        
        public async Task UpdateUserAsync(string id, string name)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
                

            user.Name = name;
            await _userManager.UpdateAsync(user);
        }

        
        public async Task ChangeUserRoleAsync(string userId, UserRole newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!await _roleManager.RoleExistsAsync(newRole.ToString()))
            {
                await _roleManager.CreateAsync(
                    new IdentityRole(newRole.ToString()));
            }

            await _userManager.AddToRoleAsync(user, newRole.ToString());
        }

       
        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            await _userManager.DeleteAsync(user);
        }

    }
}

using BusinessLayer.Enum;
using BusinessLayer;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityContext = DataLayer.IdentityContext;

namespace ServiceLayer
{
    public class IdentityService
    {
        private readonly DataLayer.IdentityContext identityContext;
        private readonly RoleManager<IdentityRole> roleManager;

        public IdentityService(
            IdentityContext identityContext,
            RoleManager<IdentityRole> roleManager)
        {
            this.identityContext = identityContext;
            this.roleManager = roleManager;
        }

        // REGISTER
        public async Task RegisterAsync(
            string email,
            string name,
            string password,
            UserRole role)
        {
            var user = new User
            {
                UserName = email,
                Email = email,
                Name = name,
                Role = role
            };

            var result = await identityContext.CreateAsync(user, password);

            if (!result.Succeeded)
                throw new Exception(
                    string.Join("; ", result.Errors.Select(e => e.Description)));

            if (!await roleManager.RoleExistsAsync(role.ToString()))
                await roleManager.CreateAsync(
                    new IdentityRole(role.ToString()));

            await identityContext.AddToRoleAsync(user, role.ToString());
        }

        // LOGIN
        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = identityContext.ReadAll()
                .FirstOrDefault(u => u.Email == email);

            if (user == null) return null;

            bool valid = await identityContext.CheckPasswordAsync(user, password);
            return valid ? user : null;
        }

        // ADMIN CRUD
        public async Task<List<User>> GetAllAsync()
            => identityContext.ReadAll().ToList();

        public async Task<User?> GetByIdAsync(string id)
            => await identityContext.ReadAsync(id);

        public async Task UpdateAsync(string id, string name)
        {
            var user = await identityContext.ReadAsync(id);
            if (user == null) throw new Exception("User not found");

            user.Name = name;
            await identityContext.UpdateAsync(user);
        }

        public async Task DeleteAsync(string id)
        {
            var user = await identityContext.ReadAsync(id);
            if (user == null) return;

            await identityContext.DeleteAsync(user);
        }
    }
}

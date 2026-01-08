using BusinessLayer;
using BusinessLayer.Enum;
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
        private readonly UserManager<User> userManager;

        public IdentityContext(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

       
        public async Task<IdentityResult> CreateAsync(User user, string password)
  
           => await userManager.CreateAsync(user, password);
           


        // READ
        public async Task<User> ReadAsync(string id)
            => await userManager.FindByIdAsync(id);
        

        public IQueryable<User> ReadAll()
            => userManager.Users;

        // UPDATE
        public async Task UpdateAsync(User user)
            => await userManager.UpdateAsync(user);

        // DELETE
        public async Task DeleteAsync(User user)
            => await userManager.DeleteAsync(user);

        // AUTH
        public async Task<bool> CheckPasswordAsync(User user, string password)
            => await userManager.CheckPasswordAsync(user, password);

        public async Task AddToRoleAsync(User user, string role)
            => await userManager.AddToRoleAsync(user, role);

        public async Task<IList<string>> GetRolesAsync(User user)
            => await userManager.GetRolesAsync(user);
    }
}

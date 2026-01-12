using BusinessLayer;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class AdminService
    {
        private readonly SchoolSuppliesDbContext context;

        public AdminService(SchoolSuppliesDbContext context)
        {
            this.context = context;
        }


        public List<User> GetAllUsers()
        {
            return context.Users
                .Include(u => u.Role)
                .ToList();
        }

        public void CreateUser(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = context.Users.Find(id);
            if (user != null)
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }
    }
}

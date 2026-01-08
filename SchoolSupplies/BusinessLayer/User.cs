using BusinessLayer.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BusinessLayer
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } 
        public List<Software> Softwares { get; set; } = new List<Software>();
        public List<Hardware> Hardwares { get; set; } = new List<Hardware>();
        public UserRole Role { get; set; }
        public User()
        {
        }

        public User(string email, string name, string password,UserRole role=UserRole.User)
        {
            Email = email;
            Role = role;
            Name = name;
            var passwordHasher = new PasswordHasher<User>();
            PasswordHash = passwordHasher.HashPassword(this, password);
        }
        public User(string email,string name,string password,List<Hardware> hardwares, List<Software> softwares ,UserRole role = UserRole.User) : this(email, name, password, role)
        {
            Hardwares = hardwares;
            Softwares = softwares;
        }

    }
}

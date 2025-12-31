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
        public ICollection<Item> Items { get; set; } = new List<Item>();
        public User()
        {
        }

        public User(string email, string name)
        {
            Email = email;
            NormalizedEmail = email.ToUpper();
            UserName = email;
            NormalizedUserName = email.ToUpper();
            Name = name;
        }
    }
}

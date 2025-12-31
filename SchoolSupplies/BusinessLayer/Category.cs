using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Category
    {
        [Key]   
        public int Id { get; set; }

        [Required]      
        [MaxLength(50, ErrorMessage = "Name must not be more than 50 symbols!")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 symbols!")]
        public string Name { get; set; }
        public ICollection<Item> Items { get; set; } = new List<Item>();
        public Category() { }
        public Category(string name)
        {
            Name = name;
        }
    }
}

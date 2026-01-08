using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class MaintenanceLog
    {
        [Key]
        public int Id { get; set; }
        public Software Software{ get; set; }
        public Hardware Hardware{ get; set; }
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500, ErrorMessage = "Description cannot more than 500 symbols")]
        public string Description { get; set; }
        public DateTime Date { get; set; } 
        public User User { get; set; }


        public MaintenanceLog() { }
        public MaintenanceLog(string description,DateTime date,User user)
        {
            Description = description;
            Date = DateTime.UtcNow;
            User = user;
        }

    }
}

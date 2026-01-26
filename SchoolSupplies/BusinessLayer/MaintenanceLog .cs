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
        [Required(ErrorMessage = "Описанието е задължително!")]
        [MaxLength(500, ErrorMessage = "Описанието не може да бъде над 500 символа!")]
        public string Description { get; set; }
        public DateTime Date { get; set; } 

        public MaintenanceLog() { }
        public MaintenanceLog(string description,DateTime date)
        {
            Description = description;
            Date = DateTime.UtcNow;
         
        }

    }
}

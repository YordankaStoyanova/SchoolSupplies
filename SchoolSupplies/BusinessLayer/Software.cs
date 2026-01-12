using BusinessLayer.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Software
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Name must not be more than 50 symbols!")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 symbols!")]
        public string Name { get; set; }

        

        [Required(ErrorMessage = "Serial number is required")]
        [StringLength(100, MinimumLength = 5,
     ErrorMessage = "Serial number must be between 5 and 100 symbols")]
        [RegularExpression(@"^[A-Za-z0-9]+$",
     ErrorMessage = "Serial number can contain only letters and numbers")]
        public string SerialNumber { get; set; }

        public Category Category { get; set; }
        [Required]
        public Type Type { get; set; }
        [Required]
        public User User { get; set; }
        public List<MaintenanceLog> MaintenanceLogs { get; set; } = new List<MaintenanceLog>();
        public int LicenseId { get; set; }
        [Required]
        public License License { get; set; }
        public List <Hardware> Hardwares { get; set; }
        public Software ()
        {

        }
    }
}

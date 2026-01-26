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
        [MaxLength(50, ErrorMessage = "Името не може да е над 50 символа!")]
        [MinLength(2, ErrorMessage = "Името трябва да е поне 2 символа!")]
        [RegularExpression(@"^(\w)+$")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Серийният номер е задължителен!")]
        [StringLength(100, MinimumLength = 5,
     ErrorMessage = "Серийният номер трябва да е между 5 и 100 символа!")]
        [RegularExpression(@"^[A-Za-z0-9]+$",
     ErrorMessage = "Серийният номер може да съдържа само букви и числа!")]
        public string SerialNumber { get; set; }

        [Required]
        public Type Type { get; set; }
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

using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.ViewModels
{
    public class SoftwareViewModel
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Името не може да е над 50 символа!")]
        [MinLength(2, ErrorMessage = "Името трябва да е поне 2 символа!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Серийният номер е задължителен!")]
        [StringLength(100, MinimumLength = 5,
        ErrorMessage = "Серийният номер трябва да е между 5 и 100 символа!")]
        [RegularExpression(@"^[A-Za-z0-9\-\/]+$",
       ErrorMessage = "Серийният номер може да съдържа букви,числа, - и / ")]
        public string SerialNumber { get; set; }

        [Required(ErrorMessage = "Избери вид!")]
        public int TypeId { get; set; }
        //  public List<MaintenanceLog> MaintenanceLogs { get; set; } = new List<MaintenanceLog>();
        [Required(ErrorMessage = "Избери лиценз!")]
        public int LicenseId { get; set; }
        
        public List<int> HardwareIds { get; set; } = new List<int> { };
       
    }
}

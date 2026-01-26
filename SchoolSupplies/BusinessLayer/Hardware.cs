using BusinessLayer.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Hardware
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Името е задължително!")]
        [MaxLength(50, ErrorMessage = "Името не може да бъде повече от 50 символа!")]
        [MinLength(2, ErrorMessage = "Името трябва да бъде поне два символа!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Инвентарният номер е задължителен!")]
        [StringLength(50, MinimumLength = 3,
    ErrorMessage = "Инвентарният номер трябва да е между 3 и 50 символа!")]
        [RegularExpression(@"^[A-Za-z0-9\-\/]+$",
    ErrorMessage = "Инвентарният номер може да съдържа букви,числа, - и / ")]
        public string InventoryNumber { get; set; }

        [Required(ErrorMessage = "Серийният номер е задължителен!")]
        [StringLength(100, MinimumLength = 5,
     ErrorMessage = "Серийният номер трябва да е между 5 и 100 символа!")]
        [RegularExpression(@"^[A-Za-z0-9]+$",
     ErrorMessage = "Серийният номер трябва да съдържа само букви и числа!")]
        public string SerialNumber { get; set; }

        public Type Type { get; set; }

        public Room Room { get; set; }

        [Required]
        public ItemStatus Status { get; set; }

        public List<MaintenanceLog> MaintenanceLogs { get; set; } = new List<MaintenanceLog>();
        public List<Software> Softwares { get; set; }= new List<Software>();
        public Hardware()
        {

        }

    }
}

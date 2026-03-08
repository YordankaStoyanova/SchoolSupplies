using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.ViewModels
{
    public class MaintenanceViewModel
    {
        public int ParentId { get; set; }
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Описанието е задължително!")]
        [MaxLength(500, ErrorMessage = "Описанието не може да бъде над 500 символа!")]
        public string Description { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Now;
    }
}

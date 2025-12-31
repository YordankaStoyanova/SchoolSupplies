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
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500, ErrorMessage = "Description cannot more than 500 symbols")]
        public string Description { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public MaintenanceLog() { }
        public MaintenanceLog(int itemId, string description)
        {
            ItemId = itemId;
            Description = description;
            Date = DateTime.Now;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Name must not be more than 50 symbols!")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 symbols!")]   
        public string Name { get; set; }

        public string InventoryNumber { get; set; } = null!;
        public string SerialNumber { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string Specifications { get; set; } = null!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;

        public ItemStatus Status { get; set; }

        public string? AssignedUserId { get; set; }
    }
}

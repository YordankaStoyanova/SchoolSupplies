using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Required(ErrorMessage = "Inventory number is required")]
        [StringLength(50, MinimumLength = 3,
    ErrorMessage = "Inventory number  must be between  3 and 50 symbols")]
        [RegularExpression(@"^[A-Za-z0-9\-\/]+$",
    ErrorMessage = "Inventory number can contain only letters, numbers, - and / ")]
        public string InventoryNumber { get; set; }

        [Required(ErrorMessage = "Serial number is required")]
        [StringLength(100, MinimumLength = 5,
     ErrorMessage = "Serial number must be between 5 and 100 symbols")]
        [RegularExpression(@"^[A-Za-z0-9]+$",
     ErrorMessage = "Serial number can contain only letters and numbers")]
        public string SerialNumber { get; set; }
       

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;

        [Required]
        public ItemStatus Status { get; set; }
        [ForeignKey("User")]
        public string? UserId { get; set; } 
        public User? User { get; set; }
        public ICollection<MaintenanceLog> MaintenanceLogs { get; set; } = new List<MaintenanceLog>();
        public ICollection<License> Licenses { get; set; } = new List<License>();
        public Item()
        {

        }
        public Item(string name, string inventoryNumber, string serialNumber,
              int categoryId, int roomId, ItemStatus status)
        {
            Name = name;
            InventoryNumber = inventoryNumber;
            SerialNumber = serialNumber;
            CategoryId = categoryId;
            RoomId = roomId;
            Status = status;
        }
        public Item(string name, string inventoryNumber, string serialNumber,
            int categoryId, int roomId, ItemStatus status, string? userId = null )
        {
            Name = name;
            InventoryNumber = inventoryNumber;
            SerialNumber = serialNumber;
            CategoryId = categoryId;
            RoomId = roomId;
            Status = status;
            UserId = userId;
        }
    }
}

using BusinessLayer.Enum;
using BusinessLayer;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class HardwareViewModel
    {
            public int Id { get; set; }

            [Required(ErrorMessage = "Name is required")]
            [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
            public string Name { get; set; }

            [StringLength(50, ErrorMessage = "Serial number cannot exceed 50 characters")]
            public string SerialNumber { get; set; }

            [Required(ErrorMessage = "Category is required")]
            public Category Category { get; set; }

            [Required(ErrorMessage = "Status is required")]
            public ItemStatus Status { get; set; }

            [Required(ErrorMessage = "Type is required")]
            [Display(Name = "Type")]
            public int TypeId { get; set; }
            public BusinessLayer.Type Type { get; set; }

            [Required(ErrorMessage = "Room is required")]
            [Display(Name = "Room")]
            public int RoomId { get; set; }
            public Room Room { get; set; }

            public List<int> SelectedSoftwareIds { get; set; } = new List<int>();
            public List<Software> AvailableSoftwares { get; set; } = new List<Software>();
        }
    }

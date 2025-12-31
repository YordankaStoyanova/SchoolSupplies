using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(1, 300, ErrorMessage = "Room number must be in [1;300]")]
        public int RoomNumber { get; set; }
        [Required]
        [Range(1, 3, ErrorMessage = "Floor must be in [1;3]")]
        public int Floor { get; set; }
        public ICollection<Item> Items { get; set; } = new List<Item>();
        public Room()
        {

        }
        public Room(int roomNumber, int floor)
        {
            RoomNumber = roomNumber;
            Floor = floor;
        }
    }
}

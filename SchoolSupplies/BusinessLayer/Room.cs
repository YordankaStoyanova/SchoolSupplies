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
        public List<Software> Softwares { get; set; } = new List<Software>();
        public List<Hardware> Hardwares { get; set; } = new List<Hardware>();
        public Room()
        {

        }
        public Room(int roomNumber, int floor)
        {
            RoomNumber = roomNumber;
            Floor = floor;
        }
        public Room(int roomNumber,int floor,List<Software> softwares,List<Hardware> hardwares):this(roomNumber,floor)
        {
           Softwares = softwares;
            Hardwares = hardwares;
        }
    }
}

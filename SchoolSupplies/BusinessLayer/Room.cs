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


        [Required(ErrorMessage = "Името е задължително!")]
        [MaxLength(50, ErrorMessage = "Името не може да бъде повече от 50 символа!")]
        [MinLength(2, ErrorMessage = "Името трябва да бъде поне два символа!")]
        public string Name { get; set; }
        
        public int Floor { get; set; }
        public List<Software> Softwares { get; set; } = new List<Software>();
        public List<Hardware> Hardwares { get; set; } = new List<Hardware>();
        public Room()
        {

        }
        public Room(string name, int floor)
        {
            Name = name;
            Floor = floor;
        }
        public Room(string name,int floor,List<Software> softwares,List<Hardware> hardwares):this(name,floor)
        {
           Softwares = softwares;
            Hardwares = hardwares;
        }
    }
}

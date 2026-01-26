using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Type
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Името е задължително!")]
        [MaxLength(50, ErrorMessage = "Името не може да бъде повече от 50 символа!")]
        [MinLength(2, ErrorMessage = "Името трябва да бъде поне два символа!")]
        public string Name { get; set; }
        public Type() { }
        public Type(string name)
        {
            Name = name;
        }
    }
}

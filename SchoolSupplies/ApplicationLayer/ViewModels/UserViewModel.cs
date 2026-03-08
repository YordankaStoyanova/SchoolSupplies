using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.ViewModels
{
    public class UserViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Името е задължително!")]
        [MaxLength(50, ErrorMessage = "Името не може да е над 50 символа!")]
        [MinLength(2, ErrorMessage = "Името трябва да е поне 2 символа!")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Имейлът е задължителен!")]
        [EmailAddress(ErrorMessage = "Невалиден имейл!")]
        public string Email { get; set; } = string.Empty;
        [RegularExpression(@"^(08\d{8}|\+359\d{9}|359\d{9})$",
                ErrorMessage = "Телефонният номер трябва да е български и да започва с 08, +359 или 359.")]
        public string? PhoneNumber { get; set; }
    }
}

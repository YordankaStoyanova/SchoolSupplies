using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Enum;

namespace BusinessLayer
{
    public  class License
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int MaxUsage { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        // 1 License -> many Softwares
        public List<Software> Softwares { get; set; } = new();

        // общо инсталации по всички софтуери с този лиценз
        public int Usage => Softwares?.Sum(s => s.Hardwares?.Count ?? 0) ?? 0;

        public License() { }

        public License(string name, DateTime expirationDate, int maxUsage)
        {
            Name = name;
            ExpirationDate = expirationDate;
            MaxUsage = maxUsage;
        }
    }
}


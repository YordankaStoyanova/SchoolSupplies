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
        public string Name { get; set; }

        public int Usage => Software.Hardwares.Count;

        public int MaxUsage {  get; set; }
       


        [Required]
        public DateTime ExpirationDate { get; set; }

    
        public Software Software { get; set; }

        [Required]
        public LicenseStatus Status { get; set; }
        public License()
        {

        }
        public License(string name, DateTime expirationDate, int maxUsage)
        {
            Name = name;
            ExpirationDate = expirationDate;
            MaxUsage = maxUsage;
        }
    }
}


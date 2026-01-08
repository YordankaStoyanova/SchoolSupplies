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

        public int Usage {  get; set; }

        public int MaxUsage {  get; set; }


        [Required]
        public DateTime ExpirationDate { get; set; }

    
        public List< Software> Softwares { get; set; } = new List< Software >();

        [Required]
        public LicenseStatus Status { get; set; }
        public License()
        {

        }
        public License(string name, DateTime expirationDate, int maxUsage,int usage=0)
        {
            Name = name;
            ExpirationDate = expirationDate;
            MaxUsage = maxUsage;
            Usage = usage;
        }
    }
}


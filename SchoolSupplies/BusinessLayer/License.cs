using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public  class License
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string LicenseKey { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public License()
        {

        }
        public License(string name, string licenseKey, DateTime expirationDate, int itemId)
        {
            Name = name;
            LicenseKey = licenseKey;
            ExpirationDate = expirationDate;
            ItemId = itemId;
        }
    }
}


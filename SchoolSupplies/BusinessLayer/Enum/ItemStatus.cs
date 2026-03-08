using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Enum
{
   public enum ItemStatus
    {
        [Display(Name = "Работещ")]
        Working = 1,

        [Display(Name = "За ремонт")]
        Repair = 2,

        [Display(Name = "Бракуван")]
        Disposed = 3
    }
}

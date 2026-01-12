using BusinessLayer.Enum;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class DashboardService
    {
        private readonly SchoolSuppliesDbContext context;

        public DashboardService(SchoolSuppliesDbContext context)
        {
            this.context = context;
        }

        public int TotalHardware()
            => context.Hardwares.Count();

        public int HardwareForRepair()
            => context.Hardwares.Count(h => h.Status == ItemStatus.Repair);
        //product.Status = ProductStatus.InStock;
        public double RepairPercentage()
        {
            int total = TotalHardware();
            if (total == 0) return 0;

            return (double)HardwareForRepair() / total * 100;
        }

        public int ExpiringLicenses()
            => context.Licenses.Count(l => l.ExpirationDate <= DateTime.Now.AddDays(30));
    }
}

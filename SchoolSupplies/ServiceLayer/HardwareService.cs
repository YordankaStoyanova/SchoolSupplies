using BusinessLayer;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class HardwareService
    {
        private readonly SchoolSuppliesDbContext context;

        public HardwareService(SchoolSuppliesDbContext context)
        {
            this.context = context;
        }

    }
}

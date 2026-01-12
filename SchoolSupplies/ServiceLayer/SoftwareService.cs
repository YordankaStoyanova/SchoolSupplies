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
    public class SoftwareService
    {
        private readonly SchoolSuppliesDbContext context;

        public SoftwareService(SchoolSuppliesDbContext context)
        {
            this.context = context;
        }


        public List<Software> GetAllSoftware()
        {
            return context.Softwares.ToList();
        }

        
    }
}

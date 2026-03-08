using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.ViewModels
{
    public class LicenseStatsViewModel
    {
        public string Title { get; set; } = string.Empty;

        public int TotalLicenses { get; set; }

        public int UsedLicenses { get; set; }
    }
}

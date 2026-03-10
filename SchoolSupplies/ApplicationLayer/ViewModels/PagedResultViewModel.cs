using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.ViewModels
{
    public class PagedResultViewModel<T>
    {
        public List<T> Items { get; set; } = new();

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }
    
}
}

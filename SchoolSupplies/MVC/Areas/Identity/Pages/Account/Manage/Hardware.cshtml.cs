using BusinessLayer;
using BusinessLayer.Enum;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;


namespace MVC.Areas.Identity.Pages.Account.Manage
{
    public class HardwareModel : PageModel
    {

        private readonly HardwareService _hardwareService;
        private readonly IDb<Room, int> _roomContext;

        public HardwareModel(HardwareService hardwareService, IDb<Room, int> roomContext)
        {
            _hardwareService = hardwareService;
            _roomContext = roomContext;
        }

        public List<Hardware> Hardwares { get; set; } = new();
        public List<Room> Rooms { get; set; } = new();

        public int WorkingCount { get; set; }
        public int RepairCount { get; set; }
        public int ScrappedCount { get; set; }

        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }

        public async Task OnGet(string? s, ItemStatus? t, int? r, int pageNumber = 1)
        {
            int pageSize = 5;

            var result = await _hardwareService.ReadPaged(pageNumber, pageSize, s, t, r);

            Hardwares = result.Items;
            PageNumber = result.CurrentPage;
            TotalPages = result.TotalPages;
            TotalItems = result.TotalItems;

            Rooms = await _roomContext.ReadAll(false, true);

            WorkingCount = await _hardwareService.HardwareWorking();
            RepairCount = await _hardwareService.HardwareRepair();
            ScrappedCount = await _hardwareService.HardwareDisposed();
        }
    }
}


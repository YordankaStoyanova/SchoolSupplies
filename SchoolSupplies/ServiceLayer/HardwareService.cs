using BusinessLayer;
using BusinessLayer.Enum;
using DataLayer;

namespace ServiceLayer
{
    public class HardwareService
    {

        private readonly IDb<Hardware, int> _context;

        public HardwareService(IDb<Hardware, int> context)
        {
            _context = context;
        }

        public async Task Create(Hardware item)
        {
            await _context.Create(item);
        }

        public async Task<Hardware> Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            return await _context.Read(key, useNavigationalProperties,isReadOnly);
        }

        public async Task<List<Hardware>> ReadAll(bool useNavigationalProperties = false,bool isReadOnly = false )
        {
            return await _context.ReadAll(useNavigationalProperties);
        }

        public async Task Update(Hardware item, bool useNavigationalProperties = false)
        {
            await _context.Update(item, useNavigationalProperties);
        }

        public async Task Delete(int key)
        {
            await _context.Delete(key);
        }
        private  List<Hardware> SearchByItemStatus(List<Hardware> hardwares,ItemStatus? itemStatus)
        {
            if (itemStatus is null) return hardwares;
            var hardwaresByStatus = hardwares.Where(h => h.Status == itemStatus.Value).ToList();
            return hardwaresByStatus;
        }
        private List<Hardware> SearchByRoom(List<Hardware> hardwares,string roomName)
        {
            if(string.IsNullOrWhiteSpace(roomName)) return hardwares;
            var hardwaresByRoom = hardwares.Where(h => h.Room.Name == roomName).ToList();
            return hardwaresByRoom;
        }

        private List<Hardware> SearchByParameter(List<Hardware> hardwares,string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter)) return hardwares;
            var hardwaresByName = hardwares
                .Where(h => h.Name == parameter || h.SerialNumber == parameter || h.InventoryNumber == parameter).ToList();
            return hardwaresByName;
        }

        private List<Hardware> SearchByDropdown(List<Hardware> hardwares,ItemStatus? status, string roomName)
        {
            List<Hardware> hardwaresStatus = SearchByItemStatus(hardwares,status);
            List<Hardware> hardwaresRoom = SearchByRoom(hardwaresStatus, roomName);
            return hardwaresRoom;
        }
        public async Task<List<Hardware>> SearchCombined(string parameter, ItemStatus? status, string roomName)
        {
            List<Hardware> hardwares = await ReadAll(true, true);
            List<Hardware> hardwaresByDropdown = SearchByDropdown(hardwares,status, roomName);
            var filteredHardwares = SearchByParameter(hardwares, parameter);
            return filteredHardwares;
        }
        public async Task<int> HardwareRepair()
        {
            List<Hardware> hardwares = await ReadAll(true, true);
            return hardwares.Count(h => h.Status == ItemStatus.Repair);
        }
        public async Task<int> TotalHardware()
        {
            List<Hardware> hardwares = await ReadAll(true, true);
            return hardwares.Count;
        }
        public async Task<int> HardwareWorking()
        {
            List<Hardware> hardwares = await ReadAll(true, true);
            return hardwares.Count(h => h.Status == ItemStatus.Working);


        }
        public async Task<int> HardwareDisposed()
        {
            List<Hardware> hardwares = await ReadAll(true, true);
            return hardwares.Count(h => h.Status == ItemStatus.Disposed);
        }
        
    }

}


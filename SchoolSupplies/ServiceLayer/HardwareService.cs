using ApplicationLayer.ViewModels;
using BusinessLayer;
using BusinessLayer.Enum;
using DataLayer;


namespace ServiceLayer
{
    public class HardwareService
    {

        private readonly IDb<Hardware, int> _hardwareContext;
        private readonly IDb<Room, int> _roomContext;
        private readonly IDb<Software, int> _softwareContext;
        private readonly IDb<BusinessLayer.Type, int> _typeContext;

        public HardwareService(IDb<Hardware, int> context, IDb<Room,int> roomContext,IDb<BusinessLayer.Type,int> typeContext,IDb<Software,int> softwareContext)
        {
            _hardwareContext = context;
            _roomContext = roomContext;
            _typeContext = typeContext;
            _softwareContext = softwareContext;
        }

        public async Task Create(HardwareViewModel item)
        {
            var type = await _typeContext.Read(item.TypeId);
            var room = await _roomContext.Read(item.RoomId);
            var softwares = new List<Software>();
            foreach (var id in item.SoftwareIds)
            {
                var software = await _softwareContext.Read(id);
                if (software != null) softwares.Add(software);
            }
            var hardware = new Hardware(item.Name, item.InventoryNumber, item.SerialNumber, type, room, item.Status, softwares);
            await _hardwareContext.Create(hardware);
        }

        public async Task<Hardware> Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            return await _hardwareContext.Read(key, useNavigationalProperties,isReadOnly);
        }

        public async Task<List<Hardware>> ReadAll(bool useNavigationalProperties = false,bool isReadOnly = false )
        {
            return await _hardwareContext.ReadAll(useNavigationalProperties);
        }

        public async Task Update(Hardware item, bool useNavigationalProperties = false)
        {
            await _hardwareContext.Update(item, useNavigationalProperties);
        }

        public async Task Delete(int key)
        {
            await _hardwareContext.Delete(key);
        }
        private  List<Hardware> SearchByItemStatus(List<Hardware> hardwares,ItemStatus? itemStatus)
        {
            if (itemStatus is null) return hardwares;
            var hardwaresByStatus = hardwares.Where(h => h.Status == itemStatus.Value).ToList();
            return hardwaresByStatus;
        }
        private List<Hardware> SearchByRoom(List<Hardware> hardwares,int? roomId)
        {
            if(roomId is null) return hardwares;
            var hardwaresByRoom = hardwares.Where(h => h.Room.Id == roomId).ToList();
            return hardwaresByRoom;
        }

        private List<Hardware> SearchByParameter(List<Hardware> hardwares,string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter)) return hardwares;
            parameter=parameter.Trim().ToLower();
            var hardwaresByName = hardwares
                .Where(h => h.Name.ToLower().Contains(parameter) || h.SerialNumber.ToLower().Contains(parameter) || h.InventoryNumber.ToLower().Contains(parameter)).ToList();
            return hardwaresByName;
        }

        private List<Hardware> SearchByDropdown(List<Hardware> hardwares,ItemStatus? status, int? roomId)
        {
            List<Hardware> hardwaresStatus = SearchByItemStatus(hardwares,status);
            List<Hardware> hardwaresRoom = SearchByRoom(hardwaresStatus, roomId);
            return hardwaresRoom;
        }
        public async Task<List<Hardware>> SearchCombined(string parameter, ItemStatus? status, int? roomId)
        {
            List<Hardware> hardwares = await ReadAll(true, true);
            List<Hardware> hardwaresByDropdown = SearchByDropdown(hardwares,status, roomId);
            var filteredHardwares = SearchByParameter(hardwaresByDropdown, parameter);
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


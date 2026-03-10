using BusinessLayer;
using BusinessLayer.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = BusinessLayer.Type;

namespace TestLayer
{
    public class TestData
    {
        public static Type CreateType(string name = "Laptop")
        {
            return new Type
            {
                Name = name
            };
        }

        public static Room CreateRoom(string name = "Lab1", int floor = 1)
        {
            return new Room
            {
                Name = name,
                Floor = floor
            };
        }

        public static License CreateLicense(string name = "Office License", int maxUsage = 10)
        {
            return new License
            {
                Name = name,
                MaxUsage = maxUsage,
                ExpirationDate = DateTime.Now.AddYears(1)
            };
        }

        public static Hardware CreateHardware(Type type, Room room, string name = "HP ProBook")
        {
            return new Hardware
            {
                Name = name,
                InventoryNumber = "INV-001",
                SerialNumber = "SERIAL-001",
                Type = type,
                Room = room,
                Status = ItemStatus.Working,
                Softwares = new List<Software>()
            };
        }

        public static Software CreateSoftware(Type type, License license, List<Hardware>? hardwares = null, string name = "Office")
        {
            return new Software
            {
                Name = name,
                SerialNumber = "SOFT-001",
                Type = type,
                TypeId = type.Id,
                License = license,
                LicenseId = license.Id,
                Hardwares = hardwares ?? new List<Hardware>(),
                MaintenanceLogs = new List<MaintenanceLog>()
            };
        }

        public static MaintenanceLog CreateMaintenanceLog(string description = "Test maintenance")
        {
            return new MaintenanceLog
            {
                Description = description,
                Date = DateTime.Now
            };
        }
    }
}

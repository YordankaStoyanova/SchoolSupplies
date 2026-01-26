using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class MaintenanceLogService
    {
        private readonly IDb<MaintenanceLog, int> _context;

        public MaintenanceLogService(IDb<MaintenanceLog, int> context)
        {
            _context = context;
        }

        public async Task Create(MaintenanceLog item)
        {
            await _context.Create(item);
        }

        public async Task<MaintenanceLog> Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            return await _context.Read(key, useNavigationalProperties, isReadOnly);
        }

        public async Task<List<MaintenanceLog>> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            return await _context.ReadAll(useNavigationalProperties,isReadOnly);
        }

        public async Task Update(MaintenanceLog item, bool useNavigationalProperties = false)
        {

            await _context.Update(item, useNavigationalProperties);
        }

        public async Task Delete(int key)
        {
            await _context.Delete(key);
        }

    }
    }


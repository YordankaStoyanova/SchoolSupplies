using BusinessLayer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = BusinessLayer.Type;

namespace DataLayer
{
    public class SchoolSuppliesDbContext : IdentityDbContext<User>
    {
        public SchoolSuppliesDbContext()
        {
        }
        public DbSet<Software> Softwares { get; set; }
        public DbSet<Hardware> Hardwares { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users {  get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<MaintenanceLog> MaintenanceLogs { get; set; }
        public DbSet<License> Licenses { get; set; }
        public SchoolSuppliesDbContext(DbContextOptions optionsBuilder) : base(optionsBuilder)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlServer("Server=DESKTOP-VICJK85\\SQLEXPRESS;Database=SchoolSupplies;Trusted_Connection=True;TrustServerCertificate=Yes;");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
        }
    }
}

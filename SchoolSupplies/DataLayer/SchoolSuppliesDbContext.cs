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
        //public DbSet<User> Users {  get; set; }
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
            modelBuilder.Entity<Software>(s =>
            {
                // 1 License -> many Softwares
                s.HasOne(x => x.License)
                 .WithMany(l => l.Softwares)
                 .HasForeignKey(x => x.LicenseId)
                 .OnDelete(DeleteBehavior.Restrict);

                // many-to-many Software <-> Hardware
                s.HasMany(x => x.Hardwares)
                 .WithMany(h => h.Softwares);

                // ако имаш TypeId:
                s.HasOne(x => x.Type)
                 .WithMany()
                 .HasForeignKey(x => x.TypeId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MaintenanceLog>(m =>
            {
                m.HasOne(x => x.Hardware)
                    .WithMany(h => h.MaintenanceLogs)
                    .HasForeignKey(x => x.HardwareId)
                    .OnDelete(DeleteBehavior.Cascade);

                m.HasOne(x => x.Software)
                    .WithMany(s => s.MaintenanceLogs)
                    .HasForeignKey(x => x.SoftwareId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            base.OnModelCreating(modelBuilder); 
        }
    }
}

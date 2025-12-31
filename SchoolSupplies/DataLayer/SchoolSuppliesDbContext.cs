using BusinessLayer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class SchoolSuppliesDbContext : IdentityDbContext<User>
    {
        public SchoolSuppliesDbContext()
        {
        }
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Room> Rooms { get; set; }
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

           
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Category)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); 

       
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Room)
                .WithMany(r => r.Items)
                .HasForeignKey(i => i.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

         
            modelBuilder.Entity<Item>()
                .HasOne(i => i.User)
                .WithMany(u => u.Items)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.SetNull); 

            modelBuilder.Entity<License>()
                .HasOne(l => l.Item)
                .WithMany(i => i.Licenses) 
                .HasForeignKey(l => l.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<MaintenanceLog>()
                .HasOne(m => m.Item)
                .WithMany(i => i.MaintenanceLogs)
                .HasForeignKey(m => m.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

         
            modelBuilder.Entity<Item>()
                .HasIndex(i => i.InventoryNumber)
                .IsUnique();

            
        }
    }
}

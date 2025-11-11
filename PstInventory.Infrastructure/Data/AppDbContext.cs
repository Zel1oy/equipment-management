using PstInventory.Core.model;
using Microsoft.EntityFrameworkCore;
using PstInventory.Core.enums;

namespace PstInventory.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Equipment> Equipment { get; set; }
    
    public DbSet<Category> Categories { get; set; }
    public DbSet<Location> Locations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Computers" },
            new Category { Id = 2, Name = "Presentation Equipment" },
            new Category { Id = 3, Name = "Peripherals" }
        );

        modelBuilder.Entity<Location>().HasData(
            new Location { Id = 1, Name = "Room 301", Address = "Main Campus" },
            new Location { Id = 2, Name = "Staff Room (302)", Address = "Main Campus" },
            new Location { Id = 3, Name = "Storage Closet", Address = "Basement" }
        );
        
        modelBuilder.Entity<Equipment>().HasData(
            new Equipment
            {
                Id = 1,
                Name = "Dell Latitude E7450 Laptop",
                InventoryNumber = "KAF-LPT-001",
                AssignedTo = "Prof. Kostenko",
                DateOfPurchase = new DateTime(2023, 5, 10).ToUniversalTime(),
                Status = EquipmentStatus.InUse,
                CategoryId = 1,
                LocationId = 1
            },
            new Equipment
            {
                Id = 2,
                Name = "Epson Projector EB-U05",
                InventoryNumber = "KAF-PRJ-001",
                AssignedTo = "N/A",
                DateOfPurchase = new DateTime(2023, 2, 15).ToUniversalTime(),
                Status = EquipmentStatus.InStock,
                CategoryId = 2,
                LocationId = 1
            },
            new Equipment
            {
                Id = 3,
                Name = "HP LaserJet Pro M404n",
                InventoryNumber = "KAF-PRN-001",
                AssignedTo = "General Use",
                DateOfPurchase = new DateTime(2022, 11, 20).ToUniversalTime(),
                Status = EquipmentStatus.UnderRepair,
                CategoryId = 3,
                LocationId = 2
            }
        );
    }
}

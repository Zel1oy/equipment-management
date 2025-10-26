using PstInventory.Core.model;
using Microsoft.EntityFrameworkCore;

namespace PstInventory.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Equipment> Equipment { get; set; }
}
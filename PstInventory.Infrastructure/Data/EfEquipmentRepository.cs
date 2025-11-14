using PstInventory.Core.model;
using PstInventory.Core.repository;

namespace PstInventory.Infrastructure.Data;

public class EfEquipmentRepository(AppDbContext context) : IEquipmentRepository
{
    public Equipment? GetById(int id)
    {
        return context.Equipment.Find(id);
    }

    public IEnumerable<Equipment> GetAll()
    {
        return context.Equipment.ToList();
    }
    
    public Equipment? FindByInventoryNumber(string inventoryNumber)
    {
        return context.Equipment
            .FirstOrDefault(e => e.InventoryNumber.ToUpper() == inventoryNumber.ToUpper());
    }

    public void Add(Equipment equipment)
    {
        context.Equipment.Add(equipment);
        context.SaveChanges();
    }

    public void Update(Equipment equipment)
    {
        context.Equipment.Update(equipment);
        context.SaveChanges();
    }

    public void Delete(Equipment equipment)
    {
        context.Equipment.Remove(equipment);
        context.SaveChanges();
    }
}

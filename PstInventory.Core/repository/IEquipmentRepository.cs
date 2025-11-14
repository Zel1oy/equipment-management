using PstInventory.Core.model;

namespace PstInventory.Core.repository;

public interface IEquipmentRepository
{
    Equipment? GetById(int id);
    IEnumerable<Equipment> GetAll();
    Equipment? FindByInventoryNumber(string inventoryNumber);
    void Add(Equipment equipment);
    void Update(Equipment equipment);
    void Delete(Equipment equipment);
}

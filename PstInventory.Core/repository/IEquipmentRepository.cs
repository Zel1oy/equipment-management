using PstInventory.Core.model;

namespace PstInventory.Core;

public interface IEquipmentRepository
{
    List<Equipment> GetAll();
    
    void SaveAll(List<Equipment> items);
}
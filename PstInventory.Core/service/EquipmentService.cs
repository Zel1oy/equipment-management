using PstInventory.Core.enums;
using PstInventory.Core.model;
using PstInventory.Core.repository;

namespace PstInventory.Core.service;

public class EquipmentService(IEquipmentRepository repository)
{
    public IEnumerable<Equipment> GetAllEquipment()
    {
        return repository.GetAll();
    }
    
    public Equipment? GetEquipmentById(int id)
    {
        return repository.GetById(id);
    }

    public void AddEquipment(string name, string inventoryNumber, string location, string assignedTo)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Equipment name cannot be empty.");
        
        if (string.IsNullOrWhiteSpace(inventoryNumber))
            throw new ArgumentException("Inventory number cannot be empty.");

        var existing = repository.FindByInventoryNumber(inventoryNumber);
        if (existing != null)
            throw new InvalidOperationException($"An item with inventory number '{inventoryNumber}' already exists.");

        var newItem = new Equipment
        {
            Name = name,
            InventoryNumber = inventoryNumber,
            Location = location,
            AssignedTo = assignedTo ?? "N/A",
            DateOfPurchase = DateTime.UtcNow,
            Status = EquipmentStatus.InStock
        };
        
        repository.Add(newItem);
    }

    public void UpdateEquipment(Equipment equipment)
    {
        if (equipment == null)
            throw new ArgumentNullException(nameof(equipment));
    
        if (string.IsNullOrWhiteSpace(equipment.Name))
            throw new ArgumentException("Equipment name cannot be empty.");

        var existing = repository.GetById(equipment.Id);
        if (existing == null)
            throw new InvalidOperationException($"No equipment found with ID {equipment.Id} to update.");
    
        var conflicting = repository.FindByInventoryNumber(equipment.InventoryNumber);
        if (conflicting != null && conflicting.Id != equipment.Id)
            throw new InvalidOperationException($"An item with inventory number '{equipment.InventoryNumber}' already exists.");

        repository.Update(equipment);
    }

    public void DeleteEquipment(int id)
    {
        var equipment = repository.GetById(id);
        if (equipment == null)
        {
            throw new InvalidOperationException($"No equipment found with ID {id} to delete.");
        }
        
        repository.Delete(equipment);
    }
}

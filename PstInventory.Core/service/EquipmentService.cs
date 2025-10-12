using PstInventory.Core.enums;
using PstInventory.Core.model;

namespace PstInventory.Core.service;

public class EquipmentService
{
    private readonly IEquipmentRepository _repository;

    private List<Equipment> _items;

    public EquipmentService(IEquipmentRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _items = _repository.GetAll();
    }
    
    public List<Equipment> GetAllEquipment()
    {
        return _items;
    }

    public Equipment? FindEquipmentByInventoryNumber(string inventoryNumber)
    {
        return string.IsNullOrWhiteSpace(inventoryNumber) ? null
            : _items.FirstOrDefault(item => item.InventoryNumber.Equals(inventoryNumber, StringComparison.OrdinalIgnoreCase));
    }
    
    public void AddEquipment(string name, string inventoryNumber, string location, string assignedTo)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Equipment name cannot be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(inventoryNumber))
            throw new ArgumentException("Inventory number cannot be empty.", nameof(inventoryNumber));

        if (FindEquipmentByInventoryNumber(inventoryNumber) != null)
            throw new InvalidOperationException($"An item with inventory number '{inventoryNumber}' already exists.");

        var nextId = _items.Count != 0 ? _items.Max(item => item.Id) + 1 : 1;

        var newItem = new Equipment
        {
            Id = nextId,
            Name = name,
            InventoryNumber = inventoryNumber,
            Location = location,
            DateOfPurchase = DateTime.Now,
            Status = EquipmentStatus.InStock,
            AssignedTo = assignedTo ?? "N/A"
        };

        _items.Add(newItem);
        _repository.SaveAll(_items);
    }
    
    public bool UpdateEquipmentStatus(string inventoryNumber, EquipmentStatus newStatus)
    {
        var itemToUpdate = FindEquipmentByInventoryNumber(inventoryNumber);

        if (itemToUpdate == null)
        {
            return false;
        }

        itemToUpdate.Status = newStatus;
        _repository.SaveAll(_items);
        return true;
    }
    
    public bool DeleteEquipment(string inventoryNumber)
    {
        var itemToDelete = FindEquipmentByInventoryNumber(inventoryNumber);

        if (itemToDelete == null)
        {
            return false;
        }

        _items.Remove(itemToDelete);
        _repository.SaveAll(_items);
        return true;
    }
}

using System.Diagnostics;
using System.Threading;
using PstInventory.Core.enums;
using PstInventory.Core.model;
using PstInventory.Core.repository;

namespace PstInventory.Core.service;

public class EquipmentService
{
    // ActivitySource для додаткового SPAN (п.3 лабораторної)
    private static readonly ActivitySource ActivitySource =
        new("PstInventory.EquipmentService");

    private readonly IEquipmentRepository _repository;

    public EquipmentService(IEquipmentRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Equipment> GetAllEquipment()
    {
        return _repository.GetAll();
    }

    public Equipment? GetEquipmentById(int id)
    {
        return _repository.GetById(id);
    }

    // Довга операція + додаткові теги для трейсингу
    public void AddEquipment(string name, string inventoryNumber, int locationId, int categoryId, string assignedTo)
    {
        using var activity = ActivitySource.StartActivity("AddEquipment-long-operation");

        activity?.SetTag("equipment.name", name);
        activity?.SetTag("equipment.categoryId", categoryId);
        activity?.SetTag("user.name", assignedTo);

        // імітація довгої операції (п.3b)
        Thread.Sleep(2000);

        // валідації
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Equipment name cannot be empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(inventoryNumber))
            throw new ArgumentException("Inventory number cannot be empty.", nameof(inventoryNumber));

        var existing = _repository.FindByInventoryNumber(inventoryNumber);
        if (existing != null)
            throw new InvalidOperationException($"An item with inventory number '{inventoryNumber}' already exists.");

        var newItem = new Equipment
        {
            Name = name,
            InventoryNumber = inventoryNumber,
            LocationId = locationId,
            CategoryId = categoryId,
            AssignedTo = assignedTo ?? "N/A",
            DateOfPurchase = DateTime.UtcNow,
            Status = EquipmentStatus.InStock
        };

        _repository.Add(newItem);
    }

    public void UpdateEquipment(Equipment equipment)
    {
        if (equipment == null)
            throw new ArgumentNullException(nameof(equipment));

        if (string.IsNullOrWhiteSpace(equipment.Name))
            throw new ArgumentException("Equipment name cannot be empty.");

        var existing = _repository.GetById(equipment.Id);
        if (existing == null)
            throw new InvalidOperationException($"No equipment found with ID {equipment.Id} to update.");

        var conflicting = _repository.FindByInventoryNumber(equipment.InventoryNumber);
        if (conflicting != null && conflicting.Id != equipment.Id)
            throw new InvalidOperationException($"An item with inventory number '{equipment.InventoryNumber}' already exists.");

        _repository.Update(equipment);
    }

    public void DeleteEquipment(int id)
    {
        var equipment = _repository.GetById(id);
        if (equipment == null)
            throw new InvalidOperationException($"No equipment found with ID {id} to delete.");

        _repository.Delete(equipment);
    }
}

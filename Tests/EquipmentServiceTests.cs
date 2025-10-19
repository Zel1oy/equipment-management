using PstInventory.Core.enums;
using PstInventory.Core.model;
using PstInventory.Core.service;

namespace Tests;

[TestFixture]
public class EquipmentServiceTests
{
    [Test]
    public void AddEquipment_WithValidData_ShouldIncreaseTotalCount()
    {
        // Arrange
        var mockRepo = new MockEquipmentRepository();
        var service = new EquipmentService(mockRepo);
        var initialCount = service.GetAllEquipment().Count;

        // Act
        service.AddEquipment("New Monitor", "TEST-001", "Room 101", "N/A");
        var newCount = service.GetAllEquipment().Count;

        // Assert
        Assert.That(newCount, Is.EqualTo(initialCount + 1));
    }

    [Test] // <-- CHANGED from [Fact]
    public void AddEquipment_WithDuplicateInventoryNumber_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var initialItems = new List<Equipment>
        {
            new() { Id = 1, InventoryNumber = "EXISTING-001" }
        };
        var mockRepo = new MockEquipmentRepository(initialItems);
        var service = new EquipmentService(mockRepo);
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => 
            service.AddEquipment("Another PC", "EXISTING-001", "Room 102", "N/A")
        );
    }

    [Test]
    public void AddEquipment_WithEmptyName_ShouldThrowArgumentException()
    {
        // Arrange
        var mockRepo = new MockEquipmentRepository();
        var service = new EquipmentService(mockRepo);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            service.AddEquipment("", "TEST-002", "Room 103", "N/A")
        );
    }

    [Test]
    public void UpdateEquipmentStatus_WhenItemExists_ShouldChangeStatus()
    {
        // Arrange
        var initialItems = new List<Equipment>
        {
            new() { Id = 1, InventoryNumber = "PC-007", Status = EquipmentStatus.InUse }
        };
        var mockRepo = new MockEquipmentRepository(initialItems);
        var service = new EquipmentService(mockRepo);
        var newStatus = EquipmentStatus.UnderRepair;

        // Act
        bool result = service.UpdateEquipmentStatus("PC-007", newStatus);
        var updatedItem = service.FindEquipmentByInventoryNumber("PC-007");

        // Assert
        Assert.That(result, Is.True);
        Assert.That(updatedItem, Is.Not.Null);
        Assert.That(updatedItem.Status, Is.EqualTo(newStatus));
    }
    
    [Test]
    public void UpdateEquipmentStatus_WhenItemDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var mockRepo = new MockEquipmentRepository();
        var service = new EquipmentService(mockRepo);

        // Act
        bool result = service.UpdateEquipmentStatus("NON-EXISTENT-001", EquipmentStatus.WrittenOff);

        // Assert
        Assert.That(result, Is.False);
    }
}

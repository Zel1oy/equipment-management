using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using PstInventory.Core.enums;
using PstInventory.Core.model;
using PstInventory.Core.service;
using PstInventory.Core.repository;

namespace TestProject1.Services
{
    public class EquipmentServiceTests
    {
        private readonly Mock<IEquipmentRepository> _mockRepo;
        private readonly List<Equipment> _fakeData;
        private readonly EquipmentService _service;

        public EquipmentServiceTests()
        {
            _fakeData = new List<Equipment>
            {
                new Equipment { Id = 1, Name = "Drill", InventoryNumber = "INV001", Status = EquipmentStatus.InStock, Location = "Warehouse", AssignedTo = "John" },
                new Equipment { Id = 2, Name = "Hammer", InventoryNumber = "INV002", Status = EquipmentStatus.InUse, Location = "Workshop", AssignedTo = "Mike" }
            };

            _mockRepo = new Mock<IEquipmentRepository>();
            _mockRepo.Setup(r => r.GetAll()).Returns(_fakeData);
            _mockRepo.Setup(r => r.SaveAll(It.IsAny<List<Equipment>>()))
                     .Callback<List<Equipment>>(items => { /* тут можна логіку перевіряти */ });

            _service = new EquipmentService(_mockRepo.Object);
        }

        [Fact]
        public void GetAllEquipment_ReturnsAllItems()
        {
            var result = _service.GetAllEquipment();
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void FindEquipmentByInventoryNumber_ReturnsCorrectItem()
        {
            var result = _service.FindEquipmentByInventoryNumber("inv001");
            Assert.NotNull(result);
            Assert.Equal("Drill", result!.Name);
        }

        [Fact]
        public void FindEquipmentByInventoryNumber_ReturnsNull_ForInvalidNumber()
        {
            var result = _service.FindEquipmentByInventoryNumber("XXX999");
            Assert.Null(result);
        }

        [Fact]
        public void AddEquipment_AddsNewItem()
        {
            _service.AddEquipment("Saw", "INV003", "Storage", "Alex");

            var items = _service.GetAllEquipment();
            Assert.Equal(3, items.Count);
            Assert.Contains(items, e => e.InventoryNumber == "INV003");
            _mockRepo.Verify(r => r.SaveAll(It.IsAny<List<Equipment>>()), Times.Once);
        }

        [Fact]
        public void AddEquipment_ThrowsException_WhenInventoryNumberExists()
        {
            Assert.Throws<InvalidOperationException>(() =>
                _service.AddEquipment("Duplicate", "INV001", "Warehouse", "Sam"));
        }

        [Fact]
        public void UpdateEquipmentStatus_ChangesStatusSuccessfully()
        {
            bool updated = _service.UpdateEquipmentStatus("INV002", EquipmentStatus.Damaged);

            Assert.True(updated);
            Assert.Equal(EquipmentStatus.Damaged, _fakeData.First(e => e.InventoryNumber == "INV002").Status);
            _mockRepo.Verify(r => r.SaveAll(It.IsAny<List<Equipment>>()), Times.Once);
        }

        [Fact]
        public void UpdateEquipmentStatus_ReturnsFalse_IfItemNotFound()
        {
            bool updated = _service.UpdateEquipmentStatus("INV999", EquipmentStatus.Damaged);

            Assert.False(updated);
            _mockRepo.Verify(r => r.SaveAll(It.IsAny<List<Equipment>>()), Times.Never);
        }

        [Fact]
        public void DeleteEquipment_RemovesItemSuccessfully()
        {
            bool deleted = _service.DeleteEquipment("INV002");

            Assert.True(deleted);
            Assert.Single(_service.GetAllEquipment());
            _mockRepo.Verify(r => r.SaveAll(It.IsAny<List<Equipment>>()), Times.Once);
        }

        [Fact]
        public void DeleteEquipment_ReturnsFalse_IfItemNotFound()
        {
            bool deleted = _service.DeleteEquipment("INV999");

            Assert.False(deleted);
            Assert.Equal(2, _service.GetAllEquipment().Count);
        }
    }
}

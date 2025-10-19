using Xunit;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using PstInventory.Core.model;
using PstInventory.Core.repository;

namespace TestProject1.Repositories
{
    public class EquipmentRepositoryTests
    {
        private readonly string _testFilePath = "test_equipment.json";

        public EquipmentRepositoryTests()
        {
            // очищаємо файл перед кожним запуском тестів
            if (File.Exists(_testFilePath))
                File.Delete(_testFilePath);
        }

        [Fact]
        public void GetAll_ReturnsEmptyList_IfFileDoesNotExist()
        {
            var repo = new EquipmentRepository(_testFilePath);
            var result = repo.GetAll();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void SaveAll_CreatesFileWithData()
        {
            var repo = new EquipmentRepository(_testFilePath);
            var items = new List<Equipment>
            {
                new Equipment { Id = 1, Name = "Drill", InventoryNumber = "INV001" }
            };

            repo.SaveAll(items);

            Assert.True(File.Exists(_testFilePath));

            var json = File.ReadAllText(_testFilePath);
            var deserialized = JsonSerializer.Deserialize<List<Equipment>>(json);

            Assert.NotNull(deserialized);
            Assert.Single(deserialized);
            Assert.Equal("INV001", deserialized[0].InventoryNumber);
        }

        [Fact]
        public void GetAll_ReturnsDataFromFile()
        {
            var repo = new EquipmentRepository(_testFilePath);
            var items = new List<Equipment>
            {
                new Equipment { Id = 1, Name = "Hammer", InventoryNumber = "INV002" }
            };

            repo.SaveAll(items);

            var result = repo.GetAll();

            Assert.Single(result);
            Assert.Equal("Hammer", result[0].Name);
            Assert.Equal("INV002", result[0].InventoryNumber);
        }

        [Fact]
        public void GetAll_ReturnsEmptyList_WhenFileCorrupted()
        {
            File.WriteAllText(_testFilePath, "INVALID_JSON_CONTENT");
            var repo = new EquipmentRepository(_testFilePath);

            var result = repo.GetAll();

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}

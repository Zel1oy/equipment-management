using PstInventory.Core;
using PstInventory.Core.model;

namespace Tests;

public class MockEquipmentRepository : IEquipmentRepository
{
    private readonly List<Equipment> _items;

    public MockEquipmentRepository(List<Equipment> initialItems = null)
    {
        _items = initialItems ?? new List<Equipment>();
    }

    public List<Equipment> GetAll()
    {
        // Повертаємо копію списку, щоб тести не могли випадково змінити вихідний стан.
        return _items.ToList();
    }

    public void SaveAll(List<Equipment> items)
    {
        // Просто "зберігаємо" дані, замінюючи наш список у пам'яті.
        _items.Clear();
        _items.AddRange(items);
    }
}

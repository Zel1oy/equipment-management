using System.Text.Json;
using PstInventory.Core.model;

namespace PstInventory.Core.impl;

public class FileEquipmentRepository : IEquipmentRepository
{
    private const string FilePath = "equipment.json";

    public List<Equipment> GetAll()
    {
        if (!File.Exists(FilePath))
        {
            return [];
        }

        var json = File.ReadAllText(FilePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return [];
        }
        
        return JsonSerializer.Deserialize<List<Equipment>>(json) ?? [];
    }

    public void SaveAll(List<Equipment> items)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };

        var json = JsonSerializer.Serialize(items, options);
        
        File.WriteAllText(FilePath, json);
    }
}

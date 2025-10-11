using PstInventory.Core.enums;

namespace PstInventory.Core.model;

public class Equipment
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string InventoryNumber { get; set; }
    public string Location { get; set; }
    public DateTime DateOfPurchase { get; set; }
    public EquipmentStatus Status { get; set; }
    public string AssignedTo { get; set; }
}

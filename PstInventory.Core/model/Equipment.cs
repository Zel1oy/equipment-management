using System.ComponentModel.DataAnnotations;
using PstInventory.Core.enums;

namespace PstInventory.Core.model;

public class Equipment
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string InventoryNumber { get; set; } = null!;
    
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public int LocationId { get; set; }
    public Location Location { get; set; } = null!;
    
    public DateTime DateOfPurchase { get; set; }
    public EquipmentStatus Status { get; set; }
    
    [StringLength(100)]
    public string? AssignedTo { get; set; }
}

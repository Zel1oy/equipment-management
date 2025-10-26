using System.ComponentModel.DataAnnotations;

namespace PstInventory.WebApp.ViewModels;

public class EquipmentCreateViewModel
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100)]
    public string Name { get; set; }

    [Required(ErrorMessage = "Inventory Number is required.")]
    [StringLength(50)]
    public string InventoryNumber { get; set; }

    [StringLength(100)]
    public string Location { get; set; }

    [Display(Name = "Assigned To")]
    [StringLength(100)]
    public string? AssignedTo { get; set; }
}
using System.ComponentModel.DataAnnotations;
using PstInventory.Core.enums;

namespace PstInventory.WebApp.ViewModels;

public class EquipmentEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Inventory Number is required.")]
    [StringLength(50)]
    public string InventoryNumber { get; set; } = null!;

    [Display(Name = "Assigned To")]
    [StringLength(100)]
    public string? AssignedTo { get; set; }

    [Required]
    public EquipmentStatus Status { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a location.")]
    [Display(Name = "Location")]
    public int LocationId { get; set; }
}

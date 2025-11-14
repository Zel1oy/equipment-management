using System.ComponentModel.DataAnnotations;

namespace PstInventory.Core.model;

public class Location
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(250)]
    public string? Address { get; set; } // e.g., "Main Campus, Building A"
}
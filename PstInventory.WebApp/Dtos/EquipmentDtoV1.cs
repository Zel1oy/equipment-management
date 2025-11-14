namespace PstInventory.WebApp.Dtos;

public class EquipmentDtoV1
{
    public int Id { get; set; }                 // для відповіді
    public string? Name { get; set; }
    public string? InventoryNumber { get; set; }
    public int LocationId { get; set; }         // потрібні для AddEquipment
    public int CategoryId { get; set; }
    public string? AssignedTo { get; set; }
    public string? Status { get; set; }         // рядок з enum EquipmentStatus
}

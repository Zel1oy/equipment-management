namespace PstInventory.WebApp.Dtos;

public class EquipmentDtoV2 : EquipmentDtoV1
{
    public DateTime DateOfPurchase { get; set; }   // нового не було у v1-відповіді
    public string? Details { get; set; }           // узагальнена інфа (опціонально)
}

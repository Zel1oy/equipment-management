namespace PstInventory.WebApp.Dtos;

public class EquipmentDtoV2 : EquipmentDtoV1
{
    // додаткове поле для нової версії API
    public string? Details { get; set; }
}

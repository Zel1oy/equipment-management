using Microsoft.AspNetCore.Mvc;
using PstInventory.Core.enums;
using PstInventory.Core.model;
using PstInventory.Core.service;
using PstInventory.WebApp.Dtos;

namespace PstInventory.WebApp.Controllers;

[ApiController]
[Route("api/v2/equipment")]
public class EquipmentApiV2Controller : ControllerBase
{
    private readonly EquipmentService _service;

    public EquipmentApiV2Controller(EquipmentService service)
    {
        _service = service;
    }

    // GET api/v2/equipment
    [HttpGet]
    public IActionResult GetAll()
    {
        var items = _service.GetAllEquipment()
            .Select(e => new EquipmentDtoV2
            {
                Id = e.Id,
                Name = e.Name,
                InventoryNumber = e.InventoryNumber,
                LocationId = e.LocationId,
                CategoryId = e.CategoryId,
                AssignedTo = e.AssignedTo,
                Status = e.Status.ToString(),
                DateOfPurchase = e.DateOfPurchase,
                Details = $"Assigned to: {e.AssignedTo}, Status: {e.Status}"
            });

        return Ok(items);
    }

    // GET api/v2/equipment/5
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var e = _service.GetEquipmentById(id);
        if (e == null) return NotFound();

        var dto = new EquipmentDtoV2
        {
            Id = e.Id,
            Name = e.Name,
            InventoryNumber = e.InventoryNumber,
            LocationId = e.LocationId,
            CategoryId = e.CategoryId,
            AssignedTo = e.AssignedTo,
            Status = e.Status.ToString(),
            DateOfPurchase = e.DateOfPurchase,
            Details = $"Assigned to: {e.AssignedTo}, Status: {e.Status}"
        };

        return Ok(dto);
    }

    // POST api/v2/equipment
    [HttpPost]
    public IActionResult Create([FromBody] EquipmentDtoV2 dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        _service.AddEquipment(
            name: dto.Name!,
            inventoryNumber: dto.InventoryNumber!,
            locationId: dto.LocationId,
            categoryId: dto.CategoryId,
            assignedTo: dto.AssignedTo ?? "N/A"
        );

        var created = _service
            .GetAllEquipment()
            .First(e => e.InventoryNumber == dto.InventoryNumber);

        var result = new EquipmentDtoV2
        {
            Id = created.Id,
            Name = created.Name,
            InventoryNumber = created.InventoryNumber,
            LocationId = created.LocationId,
            CategoryId = created.CategoryId,
            AssignedTo = created.AssignedTo,
            Status = created.Status.ToString(),
            DateOfPurchase = created.DateOfPurchase,
            Details = $"Assigned to: {created.AssignedTo}, Status: {created.Status}"
        };

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // PUT api/v2/equipment/5 – оновлення повністю
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] EquipmentDtoV2 dto)
    {
        var existing = _service.GetEquipmentById(id);
        if (existing == null) return NotFound();

        existing.Name = dto.Name!;
        existing.InventoryNumber = dto.InventoryNumber!;
        existing.LocationId = dto.LocationId;
        existing.CategoryId = dto.CategoryId;
        existing.AssignedTo = dto.AssignedTo ?? "N/A";
        existing.Status = Enum.Parse<EquipmentStatus>(dto.Status ?? existing.Status.ToString());

        try
        {
            _service.UpdateEquipment(existing);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using PstInventory.Core.enums;
using PstInventory.Core.model;
using PstInventory.Core.service;
using PstInventory.WebApp.Dtos;

namespace PstInventory.WebApp.Controllers;

[ApiController]
[Route("api/v1/equipment")]
public class EquipmentApiV1Controller : ControllerBase
{
    private readonly EquipmentService _service;

    public EquipmentApiV1Controller(EquipmentService service)
    {
        _service = service;
    }

    // GET api/v1/equipment
    [HttpGet]
    public IActionResult GetAll()
    {
        var items = _service.GetAllEquipment()
            .Select(e => new EquipmentDtoV1
            {
                Id = e.Id,
                Name = e.Name,
                InventoryNumber = e.InventoryNumber,
                LocationId = e.LocationId,
                CategoryId = e.CategoryId,
                AssignedTo = e.AssignedTo,
                Status = e.Status.ToString()
            });

        return Ok(items);
    }

    // GET api/v1/equipment/5
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var e = _service.GetEquipmentById(id);
        if (e == null) return NotFound();

        var dto = new EquipmentDtoV1
        {
            Id = e.Id,
            Name = e.Name,
            InventoryNumber = e.InventoryNumber,
            LocationId = e.LocationId,
            CategoryId = e.CategoryId,
            AssignedTo = e.AssignedTo,
            Status = e.Status.ToString()
        };

        return Ok(dto);
    }

    // POST api/v1/equipment
    [HttpPost]
    public IActionResult Create([FromBody] EquipmentDtoV1 dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        _service.AddEquipment(
            name: dto.Name!,
            inventoryNumber: dto.InventoryNumber!,
            locationId: dto.LocationId,
            categoryId: dto.CategoryId,
            assignedTo: dto.AssignedTo ?? "N/A"
        );

        // після AddEquipment елемент уже є в БД – знайдемо його й повернемо
        var created = _service
            .GetAllEquipment()
            .First(e => e.InventoryNumber == dto.InventoryNumber);

        var result = new EquipmentDtoV1
        {
            Id = created.Id,
            Name = created.Name,
            InventoryNumber = created.InventoryNumber,
            LocationId = created.LocationId,
            CategoryId = created.CategoryId,
            AssignedTo = created.AssignedTo,
            Status = created.Status.ToString()
        };

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    
    // DELETE api/v1/equipment/5
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _service.DeleteEquipment(id);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}

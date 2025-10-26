using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PstInventory.Core.service;
using PstInventory.WebApp.ViewModels;

namespace PstInventory.WebApp.Controllers;

[Authorize]
public class EquipmentController(EquipmentService equipmentService) : Controller
{
    public IActionResult Index()
    {
        var equipmentList = equipmentService.GetAllEquipment();
        
        return View(equipmentList);
    }
    
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(EquipmentCreateViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        try
        {
            equipmentService.AddEquipment(
                model.Name, 
                model.InventoryNumber, 
                model.Location, 
                model.AssignedTo ?? "N/A"
            );
            
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
        }

        return View(model);
    }
    
    public IActionResult Edit(int id)
    {
        var equipment = equipmentService.GetEquipmentById(id);
        if (equipment == null)
        {
            return NotFound();
        }

        var viewModel = new EquipmentEditViewModel
        {
            Id = equipment.Id,
            Name = equipment.Name,
            InventoryNumber = equipment.InventoryNumber,
            Location = equipment.Location,
            AssignedTo = equipment.AssignedTo,
            Status = equipment.Status
        };

        return View(viewModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, EquipmentEditViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var equipmentToUpdate = equipmentService.GetEquipmentById(model.Id);
                if (equipmentToUpdate == null)
                {
                    return NotFound();
                }

                equipmentToUpdate.Name = model.Name;
                equipmentToUpdate.InventoryNumber = model.InventoryNumber;
                equipmentToUpdate.Location = model.Location;
                equipmentToUpdate.AssignedTo = model.AssignedTo;
                equipmentToUpdate.Status = model.Status;

                equipmentService.UpdateEquipment(equipmentToUpdate);
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }
        }
        
        return View(model);
    }
    
    public IActionResult Delete(int id)
    {
        var equipment = equipmentService.GetEquipmentById(id);
        if (equipment == null)
        {
            return NotFound();
        }
        return View(equipment);
    }
    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            equipmentService.DeleteEquipment(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            return RedirectToAction(nameof(Delete), new { id });
        }
    }
}
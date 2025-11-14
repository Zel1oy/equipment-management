using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PstInventory.Core.service;
using PstInventory.Infrastructure.Data;
using PstInventory.WebApp.ViewModels;

namespace PstInventory.WebApp.Controllers;

[Authorize]
public class EquipmentController(EquipmentService equipmentService, AppDbContext context) : Controller
{
    public IActionResult Index()
    {
        var equipmentList = context.Equipment
            .Include(e => e.Category)
            .Include(e => e.Location)
            .ToList();
        
        return View(equipmentList);
    }
    
    public IActionResult Create()
    {
        ViewData["CategoryId"] = new SelectList(context.Categories, "Id", "Name");
        ViewData["LocationId"] = new SelectList(context.Locations, "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(EquipmentCreateViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                equipmentService.AddEquipment(
                    model.Name, 
                    model.InventoryNumber, 
                    model.LocationId,
                    model.CategoryId,
                    model.AssignedTo
                );
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }
        }

        ViewData["CategoryId"] = new SelectList(context.Categories, "Id", "Name", model.CategoryId);
        ViewData["LocationId"] = new SelectList(context.Locations, "Id", "Name", model.LocationId);
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
            AssignedTo = equipment.AssignedTo,
            Status = equipment.Status,
            LocationId = equipment.LocationId,
            CategoryId = equipment.CategoryId
        };
        
        ViewData["CategoryId"] = new SelectList(context.Categories, "Id", "Name", viewModel.CategoryId);
        ViewData["LocationId"] = new SelectList(context.Locations, "Id", "Name", viewModel.LocationId);
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
                equipmentToUpdate.LocationId = model.LocationId;
                equipmentToUpdate.CategoryId = model.CategoryId;
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
        
        ViewData["CategoryId"] = new SelectList(context.Categories, "Id", "Name", model.CategoryId);
        ViewData["LocationId"] = new SelectList(context.Locations, "Id", "Name", model.LocationId);
        return View(model);
    }
    
    public IActionResult Delete(int id)
    {
        var equipment = context.Equipment
            .Include(e => e.Category)
            .Include(e => e.Location)
            .FirstOrDefault(e => e.Id == id);
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
    public IActionResult ApiV1Demo()
    {
        return View();
    }

    public IActionResult ApiV2Demo()
    {
        return View();
    }

}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PstInventory.Core.model;
using PstInventory.Infrastructure.Data;
using System.Threading.Tasks;

namespace PstInventory.WebApp.Controllers;

[Authorize] // Protect this whole controller
public class LocationController : Controller
{
    private readonly AppDbContext _context;

    public LocationController(AppDbContext context)
    {
        _context = context;
    }

    // GET: /Location (List Page)
    public async Task<IActionResult> Index()
    {
        var locations = await _context.Locations.ToListAsync();
        return View(locations);
    }

    // GET: /Location/Details/5 (Element-by-element Page)
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var location = await _context.Locations
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (location == null) return NotFound();

        return View(location);
    }

    // GET: /Location/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Location/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Address")] Location location)
    {
        if (ModelState.IsValid)
        {
            _context.Add(location);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(location);
    }

    // GET: /Location/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var location = await _context.Locations.FindAsync(id);
        if (location == null) return NotFound();
        
        return View(location);
    }

    // POST: /Location/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address")] Location location)
    {
        if (id != location.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(location);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Locations.Any(e => e.Id == location.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(location);
    }

    // GET: /Location/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var location = await _context.Locations
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (location == null) return NotFound();

        return View(location);
    }

    // POST: /Location/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var location = await _context.Locations.FindAsync(id);
        if (location != null)
        {
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
        }
        
        return RedirectToAction(nameof(Index));
    }
}

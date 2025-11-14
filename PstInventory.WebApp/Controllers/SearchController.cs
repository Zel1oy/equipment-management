using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PstInventory.Core.model;
using PstInventory.Infrastructure.Data;
using PstInventory.WebApp.ViewModels;
using System.Threading.Tasks;
using System.Linq;

namespace PstInventory.WebApp.Controllers;

//[Authorize]
public class SearchController : Controller
{
    private readonly AppDbContext _context;

    public SearchController(AppDbContext context)
    {
        _context = context;
    }

    // GET: /Search
    // This action just displays the empty search form
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var viewModel = new SearchViewModel
        {
            // We load the options for the dropdowns
            CategoryOptions = await GetCategoryOptionsAsync(),
            LocationOptions = await GetLocationOptionsAsync(),
            Results = new List<Equipment>() // Start with an empty list
        };
        return View(viewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(SearchViewModel viewModel)
    {
        // Start with the base query. We MUST .Include() to join the tables.
        // This satisfies requirement (iv) for JOINs.
        var query = _context.Equipment
            .Include(e => e.Category)
            .Include(e => e.Location)
            .AsQueryable(); // Make it a dynamic query

        // --- Dynamically build the query ---

        // i. Search by date and time
        if (viewModel.StartDate.HasValue)
        {
            var utcStartDate = DateTime.SpecifyKind(viewModel.StartDate.Value, DateTimeKind.Utc);
            query = query.Where(e => e.DateOfPurchase >= utcStartDate);
        }
        if (viewModel.EndDate.HasValue)
        {
            // Add a day to make the "End Date" inclusive (e.g., search all of Nov 11)
            var inclusiveEndDate = viewModel.EndDate.Value.AddDays(1);
            var utcEndDate = DateTime.SpecifyKind(inclusiveEndDate, DateTimeKind.Utc);
            query = query.Where(e => e.DateOfPurchase < utcEndDate);
        }

        // ii. Search by list of elements (Categories)
        if (viewModel.CategoryIds != null && viewModel.CategoryIds.Any())
        {
            query = query.Where(e => viewModel.CategoryIds.Contains(e.CategoryId));
        }

        // ii. Search by list of elements (Locations)
        if (viewModel.LocationIds != null && viewModel.LocationIds.Any())
        {
            query = query.Where(e => viewModel.LocationIds.Contains(e.LocationId));
        }

        // iii. Search by beginning/end of value (we'll use "Contains" as it's more useful)
        if (!string.IsNullOrEmpty(viewModel.NameSearch))
        {
            query = query.Where(e => e.Name.ToLower().Contains(viewModel.NameSearch.ToLower()));
        }

        // --- Execute the query ---
        viewModel.Results = await query.ToListAsync();
        viewModel.SearchPerformed = true;

        // --- Repopulate dropdowns (required for the form to re-display correctly) ---
        viewModel.CategoryOptions = await GetCategoryOptionsAsync();
        viewModel.LocationOptions = await GetLocationOptionsAsync();

        return View(viewModel);
    }

    // Helper method to load Category <select> options
    private async Task<IEnumerable<SelectListItem>> GetCategoryOptionsAsync()
    {
        return await _context.Categories
            .OrderBy(c => c.Name)
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToListAsync();
    }
    
    // Helper method to load Location <select> options
    private async Task<IEnumerable<SelectListItem>> GetLocationOptionsAsync()
    {
        return await _context.Locations
            .OrderBy(l => l.Name)
            .Select(l => new SelectListItem
            {
                Value = l.Id.ToString(),
                Text = l.Name
            }).ToListAsync();
    }
}

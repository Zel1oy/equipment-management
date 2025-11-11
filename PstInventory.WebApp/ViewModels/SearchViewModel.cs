using Microsoft.AspNetCore.Mvc.Rendering;
using PstInventory.Core.model;
using System.ComponentModel.DataAnnotations;

namespace PstInventory.WebApp.ViewModels;

public class SearchViewModel
{
    [Display(Name = "Name contains...")]
    public string? NameSearch { get; set; }

    [Display(Name = "Purchased After")]
    [DataType(DataType.Date)]
    public DateTime? StartDate { get; set; }

    [Display(Name = "Purchased Before")]
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Categories")]
    public List<int>? CategoryIds { get; set; }

    [Display(Name = "Locations")]
    public List<int>? LocationIds { get; set; }

    public IEnumerable<SelectListItem>? CategoryOptions { get; set; }
    public IEnumerable<SelectListItem>? LocationOptions { get; set; }

    
    public IEnumerable<Equipment>? Results { get; set; }

    public bool SearchPerformed { get; set; } = false;
}

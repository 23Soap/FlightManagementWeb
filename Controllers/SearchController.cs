using FlightManagementWeb.Data;
using FlightManagementWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FlightManagementWeb.Controllers;

public class SearchController : Controller
{
    
    public readonly ApplicationDbContext _context;
    public readonly UserManager<ApplicationUser> _userManager;

    public SearchController(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> SearchMenu(string departure,string arrival,int id)
    {
        var departureCities = _context.Flights
            .Where(f => f.DepartureDate > DateTime.UtcNow)
            .Select(f => f.DepartureCity)
            .Distinct()
            .ToList();
        
        var userName = await _userManager.GetUserAsync(User);
        ViewBag.Welcome = userName?.FirstName + " " + userName?.LastName;
        
        ViewBag.departureCitiesForDropDown = new SelectList(departureCities);

        if (!string.IsNullOrEmpty(departure))
        {
            var arrivalCity = _context.Flights
                .Where(f=> f.DepartureCity == departure)
                .Where(f=> f.DepartureDate > DateTime.UtcNow)
                .Select(f => f.ArrivalCity)
                .Distinct()
                .ToList();
            
            ViewBag.arrivalCitiesForDropDown = new SelectList(arrivalCity);
            ViewBag.SelectedDepartureCity = departure;
            ViewBag.SelectedArrivalCity = arrival;
        }

        var final = _context.Flights.Include(a => a.Aircraft).Where(d => d.DepartureCity == departure).Where(a => a.ArrivalCity == arrival)
            .ToList();
        
        ViewBag.FlightResults = final;
        ViewBag.Count = final.Count;
        ViewBag.SelectedArrivalCity = arrival;
        return View();
    }
    
   
}
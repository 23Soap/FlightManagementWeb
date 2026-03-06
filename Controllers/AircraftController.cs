using FlightManagementWeb.Data;
using FlightManagementWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagementWeb.Controllers;

public class AircraftController : Controller
{
    private readonly ApplicationDbContext _context;

    public AircraftController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult AircraftMenu()
    {
        return View();
    }

    [HttpGet]
    public IActionResult CreateAircraft()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult>  CreateAircraft(Aircraft aircraft)
    {
        
        if (ModelState.IsValid)
        {
            _context.Aircrafts.Add(aircraft);
            await _context.SaveChangesAsync();
        }
        return View(aircraft);
    }
}
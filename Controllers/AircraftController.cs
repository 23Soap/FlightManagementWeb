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
        var aircraftList = _context.Aircrafts.ToList();
        return View(aircraftList);
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
            if (!string.IsNullOrEmpty(aircraft.AircraftModel) && !string.IsNullOrWhiteSpace(aircraft.TailNumber) && !string.IsNullOrWhiteSpace(aircraft.AirlineName))
            {
                aircraft.AircraftModel = aircraft.AircraftModel.ToUpper();
                aircraft.TailNumber = aircraft.TailNumber.ToUpper();
                aircraft.AirlineName = aircraft.AirlineName;
            }
            _context.Aircrafts.Add(aircraft);
            await _context.SaveChangesAsync();
            return RedirectToAction("AircraftMenu");
        }
        return View(aircraft);
    }
}
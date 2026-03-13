
using FlightManagementWeb.Data;
using FlightManagementWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FlightManagementWeb.Controllers;

public class FlightController : Controller
{
    private readonly ApplicationDbContext _context;

    public FlightController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    
    [HttpGet]
    public async Task<IActionResult> Admin()
    {
        var list = await _context.Flights.Include(plane => plane.Aircraft).ToListAsync();
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> SearchMenu(Flight flight)
    {

        var CityList = _context.Flights.Select(flight => flight.DepartureCity).Distinct().ToList();
        ViewBag.CitiesForDropDown = new SelectList(CityList);
        return View();
    }

    [HttpGet]
    public IActionResult CreateFlight()
    {
        var availableAircrafts = _context.Aircrafts
            .Where(a => !_context.Flights.Any(f => f.AircraftId == a.AircraftId))
            .ToList();
        
        ViewBag.AircraftList = new SelectList(availableAircrafts, "AircraftId", "TailNumber","AircraftName","AirlineName");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateFlight(Flight flight)
    {
        if (ModelState.IsValid)
        {
            flight.DepartureDate = DateTime.SpecifyKind(flight.DepartureDate, DateTimeKind.Utc);

            flight.ArrivalCity = flight.ArrivalCity.ToUpper();
            flight.DepartureCity = flight.DepartureCity.ToUpper();
            
           _context.Flights.Add(flight);
           await _context.SaveChangesAsync();
           return RedirectToAction("Admin");
        }
        
        var availableAircrafts = _context.Aircrafts
            .Where(a => !_context.Flights.Any(f => f.AircraftId == a.AircraftId))
            .ToList();
        ViewBag.AircraftList = new SelectList(availableAircrafts, "AircraftId", "TailNumber","AircraftName","AirlineName");
        
        return View(flight);
    }

    [HttpGet]
    public IActionResult EditFlight(int id)
    {
        var data = _context.Flights.FirstOrDefault(p => p.FlightId == id);
        if (data == null)
        {
            return NotFound();
        }
        
        ViewBag.AircraftList = new SelectList(_context.Aircrafts, "AircraftId", "TailNumber", "AircraftName");
        return View(data);
    }

    [HttpPost]
    public async Task<IActionResult> EditFlight(Flight flight)
    {
        if (ModelState.IsValid)
        {
            flight.DepartureDate = DateTime.SpecifyKind(flight.DepartureDate, DateTimeKind.Utc);
            flight.ArrivalCity = flight.ArrivalCity.ToUpper();
            flight.DepartureCity = flight.DepartureCity.ToUpper();
            _context.Flights.Update(flight);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Admin));
            
        }
        
        ViewBag.AircraftList = new SelectList(_context.Aircrafts, "AircraftId", "TailNumber", "AircraftName");
        return View(flight);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteFlight(int? id)
    {
        var data = await _context.Flights.Include(plane => plane.Aircraft).FirstOrDefaultAsync(plane => plane.FlightId == id);
        
        if (data == null)
        {
            return NotFound();
        }
        return View(data);
    }

    [HttpPost, ActionName("DeleteFlight")]
    public async Task<IActionResult> DeleteFlight(int id)
    {
        var data = await  _context.Flights.FirstOrDefaultAsync(plane => plane.FlightId == id);

        if (data != null)
        {
            _context.Flights.Remove(data);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Admin));
    }
    
}

using FlightManagementWeb.Data;
using FlightManagementWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FlightManagementWeb.Controllers;
[Authorize(Roles = "Admin")]
public class FlightController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    public FlightController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    
    [HttpGet]
    public async Task<IActionResult> Admin()
    {

        var adminName = await _userManager.GetUserAsync(User);
        ViewBag.AdminFName = adminName.FirstName;
        var list = await _context.Flights.Include(plane => plane.Aircraft).ToListAsync();
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> SearchMenu(string departure,string arrival,int id)
    {
        var departureCities = _context.Flights
            .Select(f => f.DepartureCity)
            .Distinct()
            .ToList();
        
        ViewBag.departureCitiesForDropDown = new SelectList(departureCities);

        if (!string.IsNullOrEmpty(departure))
        {
            var arrivalCity = _context.Flights
                .Where(f=> f.DepartureCity == departure)
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

        var availableAircrafts = _context.Aircrafts.
            Where(a => !_context.Aircrafts.Any(f => f.AircraftId == a.AircraftId) || (a.AircraftId == data.AircraftId)).ToList();
        ViewBag.AircraftList = new SelectList(availableAircrafts, "AircraftId", "TailNumber", "AircraftName");
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
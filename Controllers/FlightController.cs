using System.Data;
using FlightManagementWeb.Data;
using FlightManagementWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Index = System.Index;

namespace FlightManagementWeb.Controllers;

public class FlightController : Controller
{
    private readonly ApplicationDbContext _context;
    public FlightController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task <IActionResult> Menu(string searchString)
    {
        var departureCity = _context.Flights.Select(flights => flights.DepartureCity).Distinct().OrderBy(c => c).ToList();
        ViewBag.dCities = departureCity;
        
        var arrivalCity = _context.Flights.Select(flight => flight.ArrivalCity ).Distinct().OrderBy(c => c).ToList();
        ViewBag.aCities = arrivalCity;

        if (departureCity != null && departureCity.Count > 0)
        {
            return View(_context.Flights.ToList());
        }

        var find = _context.Flights.AsQueryable();
        if (!string.IsNullOrEmpty(searchString))
        {
            find = find.Where(flight => flight.DepartureCity.Contains(searchString));
        }
        
        var allFlights = await _context.Flights.ToListAsync();
        return View(await find.ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> Admin(string searchString)
    {
        var searchFlights = from flight in _context.Flights select flight;
        if (!string.IsNullOrEmpty(searchString))
        {
            searchFlights = searchFlights.Where(searchFlights => searchFlights.DepartureCity.Contains(searchString)||searchFlights.ArrivalCity.Contains(searchString));
        }
        
        return View(await searchFlights.ToListAsync());
    }

    [HttpGet]
    public IActionResult CreateFlight()
    {
        return View();
    }
    

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateFlight([Bind("AirlineName,DepartureCity,ArrivalCity,DepartureDate,Price,Capacity")] Flight flight)
    {
            if (ModelState.IsValid)
            {
                flight.DepartureDate = DateTime.SpecifyKind(flight.DepartureDate, DateTimeKind.Utc);
                _context.Add(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Admin));
            }
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> EditFlight(int id)
    {
        var flight = await _context.Flights.FindAsync(id);
        if (flight == null) return NotFound();
        return View(flight);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public  async Task<IActionResult> EditFlight(int id,[Bind("FlightId,AirlineName,DepartureCity,ArrivalCity,DepartureDate,Price,Capacity")] Flight flight)
    {
        if (id != flight.FlightId)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            try
            {
                flight.DepartureDate = DateTime.SpecifyKind(flight.DepartureDate, DateTimeKind.Utc);
                
                _context.Update(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Admin));
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("","Unavailable to save changes");
            }
            
        }
        return View(flight);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteFlight(int id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var flight = await _context.Flights.FirstOrDefaultAsync(flight => flight.FlightId == id);
        for (flight.FlightId  = 0; flight.FlightId < flight.FlightId; flight.FlightId--)
        {
            
        }
        if (flight == null) return NotFound();
        return View(flight);
    }

    [HttpPost, ActionName("DeleteFlight")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var flight = await _context.Flights.FindAsync(id);

        if (flight == null)
        {
            return RedirectToAction(nameof(Admin));
        }

        try
        {
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Admin));
        }
        catch (DbUpdateException e)
        {
            return RedirectToAction(nameof(DeleteFlight), new { id });
        }
    }
    
}
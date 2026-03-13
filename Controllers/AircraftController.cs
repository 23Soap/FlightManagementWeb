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
    public async Task<IActionResult> CreateAircraft(Aircraft aircraft)
    {
        
        if (ModelState.IsValid)
        {
            
            if (!string.IsNullOrEmpty(aircraft.AircraftModel) && !string.IsNullOrWhiteSpace(aircraft.TailNumber) && !string.IsNullOrWhiteSpace(aircraft.AirlineName))
            {
                aircraft.AircraftModel = aircraft.AircraftModel.ToUpper();
                aircraft.TailNumber = aircraft.TailNumber.ToUpper();
                aircraft.AirlineName = aircraft.AirlineName.ToUpper();
            }
            _context.Aircrafts.Add(aircraft);
            await _context.SaveChangesAsync();
            return RedirectToAction("AircraftMenu");
        }
        return View(aircraft);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteAircraft(int id)
    {
        var data = _context.Aircrafts.FirstOrDefault(plane => plane.AircraftId == id);

        if (id != null)
        {
            return  (View(data));
        }

        return NotFound();
    }

    [HttpPost, ActionName("DeleteAircraft")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var data = _context.Aircrafts.FirstOrDefault(plane => plane.AircraftId == id);

        if (data != null)
        {
            _context.Aircrafts.Remove(data);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("AircraftMenu");
    }

    [HttpGet]
    public async Task<IActionResult> EditAircraft(int id)
    {
        var data = _context.Aircrafts.FirstOrDefault(plane => plane.AircraftId == id);
        if (data == null)
        {
            return NotFound();
        }
        return View(data);
    }

    [HttpPost, ActionName("EditAircraft")]
    public async Task<IActionResult> EditAircraft(Aircraft aircraft)
    {

        if (ModelState.IsValid)
        {
            if (!string.IsNullOrEmpty(aircraft.AircraftModel) && !string.IsNullOrWhiteSpace(aircraft.TailNumber) && !string.IsNullOrWhiteSpace(aircraft.AirlineName))
            {
                aircraft.AircraftModel = aircraft.AircraftModel.ToUpper();
                aircraft.TailNumber = aircraft.TailNumber.ToUpper();
                aircraft.AirlineName = aircraft.AirlineName.ToUpper();
                
                _context.Aircrafts.Update(aircraft);
                await _context.SaveChangesAsync();
        
                
            }
            return RedirectToAction("AircraftMenu");
        }
        return View(aircraft);
    }
}
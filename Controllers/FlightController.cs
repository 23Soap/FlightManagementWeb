
using FlightManagementWeb.Data;
using FlightManagementWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FlightManagementWeb.Controllers;

public class FlightController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public FlightController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Admin(string email)
    {
        var adminName = await _userManager.GetUserAsync(User);
        ViewBag.AdminFName = adminName.FirstName;
        
        var allRoles = await _roleManager.Roles.ToListAsync();
        ViewBag.allRoles = allRoles;

        if (!string.IsNullOrEmpty(email))
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user !=null)
            {
                ViewBag.emailFinded = user;
                var getUserRole = await _userManager.GetRolesAsync(user);
                var getUserPurchases = await _context.Purchases.Where(m => m.UserId == user.Id)
                    .Include(f => f.Flight)
                    .ThenInclude(a => a.Aircraft)
                    .ToListAsync();
                ViewBag.userPurchases = getUserPurchases;
                ViewBag.userRole = getUserRole;
                var archivedFlights = await _context.ArchivedPurchases.Where(m => m.UserId == user.Id)
                    .Include(f => f.ArchivedFlight)
                    .ToListAsync();
                ViewBag.archivedFlights = archivedFlights;
            }
            else
            {
                TempData["UserAlert"] = "Please Enter a Valid E-Mail, Try again! ";
            }
        }
        
        var list = await _context.Flights.Include(plane => plane.Aircraft).ToListAsync();
        return View(list);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeRoles(string id,string email)
    {
        try
        {
            var role = await _roleManager.FindByIdAsync(id);
            var user = await _userManager.FindByEmailAsync(email);
            var currentRole = await _userManager.GetRolesAsync(user);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role With Id = {id} cannot be found";
                return RedirectToAction("Admin");
            }
            else
            {
                await _userManager.RemoveFromRolesAsync(user, currentRole);
                await _userManager.AddToRoleAsync(user, role.Name!);
                TempData["RoleAlert"] = "User Role is Successfully Updated";
                return RedirectToAction("Admin");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        await _userManager.DeleteAsync(user!);
        TempData["DeleteAlert"] = "User is Deleted Successfully";
        return RedirectToAction("Admin");
    }

    [HttpPost]
    public async Task<IActionResult> RefundTicket(int id)
    {
        var ticket = await _context.Purchases.Include(p => p.Flight.Aircraft)
            .SingleOrDefaultAsync(a => a.PurchaseNumber == id);
        if (ticket == null)
        {
            return NotFound();
        }
        
        ticket.Flight.Aircraft.Capacity +=1;
        _context.Purchases.Remove(ticket);
        await _context.SaveChangesAsync();
        return  RedirectToAction("Admin");
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
    
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult CreateFlight()
    {
        var availableAircrafts = _context.Aircrafts
            .Where(a => !_context.Flights.Any(f => f.AircraftId == a.AircraftId))
            .ToList();
        
        ViewBag.AircraftList = new SelectList(availableAircrafts, "AircraftId", "TailNumber","AircraftName","AirlineName");
        return View();
    }
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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
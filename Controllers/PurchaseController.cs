using FlightManagementWeb.Data;
using FlightManagementWeb.Models;
using FlightManagementWeb.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightManagementWeb.Controllers;

public class PurchaseController : Controller
{
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly PurchaseService _purchaseService;
        public readonly ApplicationDbContext _context;

        public PurchaseController(PurchaseService purchaseService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _purchaseService = purchaseService;
            _context = context;
        }
        
        [HttpGet]
        public IActionResult BuyFlight(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var data = _context.Flights.Include(f => f.Aircraft)
                .FirstOrDefault(p=> p.FlightId == id);
            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }
    
    [HttpGet]
    public IActionResult Purchase (int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var data = _context.Flights.
            Include(f=> f.Aircraft)
            .SingleOrDefault(p => p.FlightId == id);
        if (data == null)
        {
            return NotFound();
        }

        var purchase = new Purchase
        {
            Flight = data,
            FlightId = data.FlightId
        };
        
        return View(purchase);
    }

    [HttpPost]
    public async Task<IActionResult> Purchase(int id,Purchase purchase)
    {
        if (!ModelState.IsValid)
        {
            await  LoadFlightAsync(purchase, purchase.FlightId);
            return View(purchase);
        }
        try
        {
            
            var userId = _userManager.GetUserId(User);
            await _purchaseService.CreatePurchaseAsync(userId, id, purchase); 
            return RedirectToAction("PurchaseComplate","Purchase");
            
        }
        catch (InvalidOperationException e)
        {
            ModelState.AddModelError("", e.Message);
            await LoadFlightAsync(purchase, purchase.FlightId);
            return View(purchase);
        }
    }
    [HttpGet]
    public IActionResult PurchaseComplate()
    {
        return View();
    }
    
    private async Task LoadFlightAsync(Purchase purchase,int flightId)
    {
        purchase.Flight = await _context.Flights.Include(f => f.Aircraft).FirstOrDefaultAsync(f => f.FlightId == purchase.FlightId);
    }
    
    
}
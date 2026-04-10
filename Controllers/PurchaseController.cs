using FlightManagementWeb.Data;
using FlightManagementWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightManagementWeb.Controllers;

public class PurchaseController : Controller
{
        public readonly ApplicationDbContext _context;
        public readonly UserManager<ApplicationUser> _userManager;

        public PurchaseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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

        [HttpPost]
        public async Task<IActionResult> BuyFlight(int id)
        {
            var data = _context.Flights.Include(a => a.Aircraft)
                .SingleOrDefault(f => f.FlightId == id);
            
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
        
        var userr = _context.Flights.Include(f => f.Aircraft).SingleOrDefault(f => f.FlightId == id);
        
        if (userr == null)
        {
            return NotFound();
        }
        
        if (ModelState.IsValid)
        {
            purchase.FirstName = purchase.FirstName.ToUpper();
            purchase.LastName = purchase.LastName.ToUpper();
            purchase.Email = purchase.Email.ToUpper();
            purchase.AdressLine1 = purchase.AdressLine1.ToUpper();
            purchase.CardNumber = purchase.CardNumber.ToUpper();
            purchase.Expiration = purchase.Expiration.ToString();
            purchase.CCVNumber = purchase.CCVNumber.ToUpper();
            purchase.Country = purchase.Country.ToUpper();
            purchase.NameOnTheCard = purchase.NameOnTheCard.ToUpper();
            purchase.State = purchase.State.ToUpper();
            purchase.ZipCode = purchase.ZipCode.ToUpper();
            purchase.FlightId = id;
            purchase.UserId = _userManager.GetUserId(User);
            
            purchase.UserId = _userManager.GetUserId(User);
            Guid newGuid = new Guid();
            do
            {
                purchase.PurchaseNumber = Guid.NewGuid().GetHashCode();
                
            } while (_context.Purchases.Any(f => f.PurchaseNumber == purchase.PurchaseNumber));
            
            purchase.Flight = null;
            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();
            return RedirectToAction("PurchaseComplate","Purchase");
        }
        purchase.Flight = userr;
        return View(purchase);
    }
    [HttpGet]
    public IActionResult PurchaseComplate()
    {
        return View();
    }
    
    
}
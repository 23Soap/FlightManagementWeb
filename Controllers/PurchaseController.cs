using FlightManagementWeb.Data;
using FlightManagementWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightManagementWeb.Controllers;

public class PurchaseController : Controller
{
        public readonly ApplicationDbContext _context;

        public PurchaseController(ApplicationDbContext context)
        {
            _context = context;
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
            FlightNumber = data.FlightId
        };
        
        return View(purchase);
    }

    [HttpPost]
    public async Task<IActionResult> Purchase(int id,Purchase purchase)
    {
        
        var userr = _context.Flights.SingleOrDefault(p => p.FlightId == id);

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

            purchase.FlightNumber = id;
            purchase.PurchaseNumber = purchase.PurchaseNumber;
            
            _context.Purchases.Add(purchase);
            await  _context.SaveChangesAsync();
            return RedirectToAction("SearchMenu","Flight");
        }

        return View(purchase);
    }
}
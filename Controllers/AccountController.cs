using FlightManagementWeb.Data;
using FlightManagementWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagementWeb.Controllers;

public class AccountController : Controller
{
    
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount(ApplicationUser appUser)
    {
        if (ModelState.IsValid)
        {
            
            await _context.SaveChangesAsync();
            return RedirectToAction();
        }
        return  View();
    }
    
    
}
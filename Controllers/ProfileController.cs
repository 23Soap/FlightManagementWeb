using FlightManagementWeb.Data;
using FlightManagementWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightManagementWeb.Controllers;

public class ProfileController : Controller
{
    private readonly ILogger<ProfileController> _logger;
    
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    public ProfileController(ILogger<ProfileController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }
    [Authorize(Roles = "User,Admin,Manager")]
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        var final = await _context.Purchases.Include(f => f.Flight)
            .ThenInclude(a => a.Aircraft)
            .Where(f => f.UserId == user.Id)
            .ToListAsync();

        var viewModel = new Profile
        {
            User = user,
            Purchase = final
        };
        
        return View(viewModel);
    }
}
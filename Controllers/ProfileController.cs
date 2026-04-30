using FlightManagementWeb.Data;
using FlightManagementWeb.Models;
using FlightManagementWeb.Services;
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
    private readonly PurchaseService _purchaseService;
    public ProfileController(ILogger<ProfileController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager,PurchaseService purchaseService)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _purchaseService =  purchaseService;
    }
    [Authorize(Roles = "User,Admin,Manager")]
[HttpGet]
public async Task<IActionResult> Profile()
{
    var user = await _userManager.GetUserAsync(User);

    var purchases = await _context.Purchases
        .Include(f => f.Flight)
            .ThenInclude(a => a.Aircraft)
        .Where(f => f.UserId == user.Id)
        .ToListAsync();
    
    var nextFlight = purchases.Where(g => g.Flight.DepartureDate >= DateTime.UtcNow).ToList();
    var todayFlight = purchases.Where(g => g.Flight.DepartureDate.Date == DateTime.UtcNow.Date).ToList();

    var previousFlight = await _context.ArchivedPurchases
        .Include(a => a.Flight)
            .ThenInclude(f => f.Aircraft)
        .Where(a => a.UserId == user.Id)
        .ToListAsync();

    
    var viewModel = new Profile
    {
        User = user,
        PreviousFlight = previousFlight,
        NextFlight = nextFlight,
        TodayFlight = todayFlight
    };
    
    return View(viewModel);
}
    [HttpGet, Authorize(Roles = "User,Admin,Manager")]
    public async Task<IActionResult> EditProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
           return NotFound();
        }
        
        var viewModel = new EditProfile
        {
            FirstName = user.FirstName, 
            LastName = user.LastName,        
            Email = user.Email,              
            PhoneNumber = user.PhoneNumber,  
            DateOfBirth = user.DateOfBirth,  
            ProfilePictureUrl = user.ProfilePictureUrl  
        };
        return View(viewModel);
    }
    
    [HttpPost, Authorize(Roles = "User,Admin,Manager")]
    public async Task<IActionResult> EditProfile(EditProfile editProfile)
    {
        if (!ModelState.IsValid)
        {
            return View(editProfile);
        }

        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }
            user.FirstName = editProfile.FirstName;
            user.LastName = editProfile.LastName;
            user.Email = editProfile.Email;
            user.PhoneNumber = editProfile.PhoneNumber;
            user.DateOfBirth = editProfile.DateOfBirth;
            user.ProfilePictureUrl = editProfile.ProfilePictureUrl;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Profile");
            }
            return View(editProfile);
    }
}
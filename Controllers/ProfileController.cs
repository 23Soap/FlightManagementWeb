using FlightManagementWeb.Data;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagementWeb.Controllers;

public class ProfileController : Controller
{
    private readonly ILogger<ProfileController> _logger;

    private readonly ApplicationDbContext _context;
    
    public ProfileController(ILogger<ProfileController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
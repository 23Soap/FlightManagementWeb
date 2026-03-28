
using FlightManagementWeb.Data;
using FlightManagementWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagementWeb.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ApplicationDbContext _context;

    public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(Register register)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = register.Email,
                Email = register.Email,
                FirstName = register.FirstName,
                LastName = register.LastName,
                DateOfBirth = register.DateOfBirth
            };
            var final =  await _userManager.CreateAsync(user, register.Password);
            if (final.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Login", "Account");
            }

            foreach (var VARIABLE in final.Errors)
            {
                ModelState.AddModelError("", VARIABLE.Description);
            }
        }

        return View(register);
    }


    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(Login login)
    {
        
        if (ModelState.IsValid)
        {
            var final = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
            if (final.Succeeded)
            {
                
                return RedirectToAction("SearchMenu", "Flight");
            }
            else
            {
               ModelState.AddModelError("", "Invalid login attempt");
            }
        }
        return View(login);
    }

    [HttpPost,Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    
    
    
    
}


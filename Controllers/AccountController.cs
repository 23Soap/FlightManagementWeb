
using FlightManagementWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagementWeb.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    

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
                return RedirectToAction("Register", "Account");
            }

            foreach (var VARIABLE in final.Errors)
            {
                ModelState.AddModelError("", VARIABLE.Description);
            }
        }

        return View(register);
    }


}
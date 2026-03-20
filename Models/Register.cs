using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FlightManagementWeb.Models;

public class Register : IdentityUser
{
    [Required(ErrorMessage = "Please enter your first name.")]
    public string FirstName { get; set; }
    [Required(ErrorMessage =  "Please enter your last name.")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Please enter your E-Mail"),EmailAddress]
    public string Email { get; set; }
    [Required(ErrorMessage = "Please Enter your Password"),DataType(DataType.Password)]
    public string Password { get; set; }
    [Required(ErrorMessage = "Please Confirm your Password"),Compare("Password",ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
    [Required(ErrorMessage = "Please Enter your date of birth")]
    public DateOnly DateOfBirth { get; set; }
}
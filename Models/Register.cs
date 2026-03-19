using System.ComponentModel.DataAnnotations;

namespace FlightManagementWeb.Models;

public class Register
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required,EmailAddress]
    public string Email { get; set; }
    [Required,DataType(DataType.Password)]
    public string Password { get; set; }
    [Required,Compare("Password",ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
}
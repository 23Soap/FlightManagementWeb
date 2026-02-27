using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FlightManagementWeb.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string FirstName {get; set;}
    [Required]
    public string LastName {get; set;}
    [Required]
    public DateOnly DateOfBirthDate {get; set;}
    
    public string? ProfilePictureUrl { get; set; }
    
    
}
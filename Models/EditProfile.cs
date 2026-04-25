namespace FlightManagementWeb.Models;

public class EditProfile
{
    public string FirstName { get; set;}
    public string LastName { get; set;}
    public string Email { get; set;}
    public string PhoneNumber { get; set;}
    public DateOnly DateOfBirth {get; set;}
    public string? ProfilePictureUrl { get; set; }
}
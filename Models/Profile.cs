namespace FlightManagementWeb.Models;

public class Profile
{
    public ApplicationUser? User { get; set; }
    public List<Purchase>? Purchase { get; set; }
}
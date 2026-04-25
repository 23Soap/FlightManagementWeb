namespace FlightManagementWeb.Models;

public class Profile
{
    // To display Users Informations
    public ApplicationUser? User { get; set; }
    // To display Previous bought flights
    public List<Purchase>? PreviousFlight { get; set; }
    // To display Next flight
    public List<Purchase>? NextFlight { get; set; }
    // To display Todays Flight
    public List<Purchase>? TodayFlight { get; set; }
}
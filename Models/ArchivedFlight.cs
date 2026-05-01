namespace FlightManagementWeb.Models;
using System.ComponentModel.DataAnnotations;
public class ArchivedFlight
{
    [Key]
    public int ArchivedFlightId { get; set; }
    public int OriginalFlightId { get; set; }
    public int OriginalAircraftId { get; set; }
    public DateTime ArchivedDate { get; set; }
    public string DepartureCity { get; set; }
    public string ArrivalCity { get; set; }
    public DateTime DepartureDate { get; set; }
    public int FlightDuration { get; set; }
    public decimal Price { get; set; }
    public string AirlineName { get; set; }
    public string TailNumber { get; set; }
    public string AircraftModel { get; set; }
    
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightManagementWeb.Models;

public class Flight
{
    [Key] 
    [Display(Name = "Flight ID")]
    public int FlightId { get; set; }
    public int AircraftId { get; set; }
    public Aircraft? Aircraft { get; set; }

    [Required]
    [Display(Name = "Departure City")]
    public string DepartureCity { get; set; }

    [Required]
    [Display(Name = "Arrival City")]
    public string ArrivalCity { get; set; }

    [Required]
    [Display(Name = "Departure Date")]
    public DateTime DepartureDate { get; set; }
    
    [Required]
    [Display(Name = "FlightDuration")]
    public int FlightDuration { get; set; }

    [Required] 
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    

}
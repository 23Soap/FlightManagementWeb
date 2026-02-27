using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightManagementWeb.Models;

public class Flight
{
    [Key] 
    [Display(Name = "Flight ID")]
    public int FlightId { get; set; }
    
    [Required]
    [Display(Name = "Airline Name")]
    public string AirlineName { get; set; }

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
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    
    [Required] 
    [Display(Name = "Capacity of Flight")]
    public int Capacity { get; set; }
    
    [Required]
    public int RemainingCapacity { get; set; }
    public int NumberOfPassengers { get; set; }

}
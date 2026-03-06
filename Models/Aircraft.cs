using System.ComponentModel.DataAnnotations;

namespace FlightManagementWeb.Models;

public class Aircraft
{
        [Key] [Required]
        [Display(Name = "Aircraft ID")]
        public int AircraftId { get; set; }
        
        [Required]
        [Display(Name = "Tail Number")]
        public string TailNumber { get; set; }
    
        [Required]
        [Display(Name = "Aircraft Model")]
        public string AircraftModel { get; set; }
    
        [Required]
        [Display(Name = "Airline Name")]
        public string AirlineName { get; set; }

    
        [Required] 
        [Display(Name = "Aircraft Capacity")]
        public int Capacity { get; set; }
        
}
using System.ComponentModel.DataAnnotations;

namespace FlightManagementWeb.Models;

public class Aircraft
{
        
        public ICollection<Flight> Flights { get; set; }
        [Key] [Required(ErrorMessage =  "Aircraft ID is required")]
        [Display(Name = "Aircraft ID")]
        public int AircraftId { get; set; }
        
        [Required(ErrorMessage = "Aircraft Name is required")]
        [Display(Name = "Tail Number"),StringLength(10)]
        public string TailNumber { get; set; }
    
        [Required(ErrorMessage =  "Aircraft Name is required")]
        [Display(Name = "Aircraft Model")]
        public string AircraftModel { get; set; }
    
        [Required(ErrorMessage =  "Aircraft Name is required")]
        [Display(Name = "Airline Name")]
        public string AirlineName { get; set; }

    
        [Required(ErrorMessage = "Aircraft Name is required")] 
        [Display(Name = "Aircraft Capacity"),Range(0,1000)]
        public int Capacity { get; set; }
        
}
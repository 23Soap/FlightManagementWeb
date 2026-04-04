using System.ComponentModel.DataAnnotations;


namespace FlightManagementWeb.Models;

public class Purchase
{
        public Flight? Flight { get; set; }
        [Key]
        public int PurchaseNumber {get; set;}
        public int FlightId {get; set;}
        
        public string? UserId {get; set;}
        
        [Required(ErrorMessage =  "Please enter your first name.")]
        public string FirstName {get; set;}
        [Required(ErrorMessage = "Please enter your last name.")]
        public string LastName {get; set;}
        [Required(ErrorMessage = "Please enter your E-Mail.")]
        public string Email {get; set;}
        [Required(ErrorMessage =  "Please enter your Address.")]
        public string AdressLine1 {get; set;}

        [Required(ErrorMessage = "Please enter your Country.")]
        public string Country {get; set;}
        [Required(ErrorMessage = "Please enter your State.")]
        public string State {get; set;}
        [Required(ErrorMessage = "Please enter your Zip Code.")]
        public string ZipCode { get; set; }
        
        public string? SaveInformations {get; set;}
        public string CardNumber {get; set;}
        public string Expiration { get; set; }
        public string CCVNumber {get; set;}
        public string NameOnTheCard {get; set;}
}
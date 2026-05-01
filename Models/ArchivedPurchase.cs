using System.ComponentModel.DataAnnotations;

namespace FlightManagementWeb.Models;

public class ArchivedPurchase
{
    public ArchivedFlight? ArchivedFlight { get; set; }
    [Key]
    public int ArchivedPurchaseId {get; set;}
    public int OriginalPurchaseNumber {get; set;}
    public DateTime ArchivedDate {get; set;} 
    public int? ArchivedFlightId { get; set; } 
    public string? UserId {get; set;}
    public int FlightId {get; set;}
    public string FirstName {get; set;}
    
    public string LastName {get; set;}
    
    public string Email {get; set;}
    
}
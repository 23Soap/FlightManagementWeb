using Microsoft.AspNetCore.Mvc;

namespace FlightManagementWeb.Models;

public class Search : Controller
{
    public Flight? Flight { get; set; }
    public Aircraft? Aircraft { get; set; }
    public Purchase? Purchase { get; set; }
}
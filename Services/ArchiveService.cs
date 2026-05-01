using FlightManagementWeb.Data;
using FlightManagementWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightManagementWeb.Services;

public class ArchiveService
{
    private readonly ApplicationDbContext _context;

    public ArchiveService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task ArchiveAll()
{
    var allPurchases = await _context.Purchases
        .Include(f => f.Flight)
        .ThenInclude(a => a.Aircraft)
        .Where(c => c.Flight.DepartureDate < DateTime.UtcNow)
        .ToListAsync();
    
    foreach (var purchase in allPurchases)
    {
        var ap = new ArchivedPurchase
        {
            OriginalPurchaseNumber = purchase.PurchaseNumber,
            ArchivedDate = DateTime.UtcNow,
            FlightId = purchase.FlightId,
            ArchivedFlightId = null, 
            UserId = purchase.UserId,
            FirstName = purchase.FirstName,
            LastName = purchase.LastName,
            Email = purchase.Email,
        };
        
        _context.ArchivedPurchases.Add(ap);
        _context.Purchases.Remove(purchase);
        purchase.Flight.Aircraft.Capacity += 1;
    }
    await _context.SaveChangesAsync();
    Console.WriteLine("Purchase arşivleme TAMAM!");
    
    var allFlights = await _context.Flights
        .Include(a => a.Aircraft)
        .Where(x => x.DepartureDate < DateTime.UtcNow)
        .ToListAsync();
    
    
    foreach (var flight in allFlights)
    {
        Console.WriteLine($"Flight {flight.FlightId} arşivleniyor...");
        
        var af = new ArchivedFlight
        {
            OriginalFlightId = flight.FlightId,
            OriginalAircraftId = flight.Aircraft.AircraftId,
            ArchivedDate = DateTime.UtcNow,
            DepartureDate = flight.DepartureDate,
            DepartureCity = flight.DepartureCity,
            ArrivalCity = flight.ArrivalCity,
            FlightDuration = flight.FlightDuration,
            Price = flight.Price,
            AircraftModel = flight.Aircraft.AircraftModel,
            AirlineName = flight.Aircraft.AirlineName,
            TailNumber = flight.Aircraft.TailNumber,
        };
        _context.ArchivedFlights.Add(af);
        await _context.SaveChangesAsync();
        

        var archivedPurchasesToUpdate = await _context.ArchivedPurchases
            .Where(ap => ap.FlightId == flight.FlightId)
            .ToListAsync();
      
        
        foreach (var ap in archivedPurchasesToUpdate)
        {
            ap.ArchivedFlightId = af.ArchivedFlightId;
        }
        
        _context.Flights.Remove(flight);
    }
    await _context.SaveChangesAsync();
}
    
}
    
using FlightManagementWeb.Data;
using FlightManagementWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightManagementWeb.Services;

public class PurchaseService
{
    private readonly ApplicationDbContext _context;

    public PurchaseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Purchase> CreatePurchaseAsync(string userId,int flightId,Purchase purchase)
    {
        var flight = await _context.Flights.Include(a => a.Aircraft).SingleOrDefaultAsync(f => f.FlightId == flightId);
        
        if (flight == null)
        {
            throw new InvalidOperationException("Flight not found");
        }
        
        if (flight.Aircraft.Capacity <= 0)
        {
            throw new InvalidOperationException("No Seats AVAILABLE");
        }
        
        purchase.FirstName = purchase.FirstName.ToUpper();
        purchase.LastName = purchase.LastName.ToUpper();
        purchase.Email = purchase.Email.ToLower();
        purchase.AdressLine1 = purchase.AdressLine1.ToUpper();
        purchase.Country = purchase.Country.ToUpper();
        purchase.State = purchase.State.ToUpper();
        purchase.ZipCode = purchase.ZipCode.ToUpper();
        purchase.NameOnTheCard = purchase.NameOnTheCard.ToUpper();
        purchase.FlightId = flightId;
        purchase.UserId = userId;
        purchase.Flight = null;
        do
        {
            purchase.PurchaseNumber = Math.Abs(Guid.NewGuid().GetHashCode());
        } while (await _context.Purchases.AnyAsync(p => p.PurchaseNumber == purchase.PurchaseNumber));
        
        flight.Aircraft.Capacity = flight.Aircraft.Capacity -1;
        
        _context.Aircrafts.Update(flight.Aircraft);
        
        _context.Purchases.Add(purchase);
        await _context.SaveChangesAsync();
        
        return purchase;
    }

    public async Task ArchiveOldFlights()
    {
        var allPurchases = await _context.Purchases.Include(f => f.Flight).ThenInclude(a => a.Aircraft).Where(c => c.Flight.DepartureDate < DateTime.UtcNow)
            .ToListAsync();

        foreach (var get in allPurchases )
        {
            var archivePurchased = new ArchivedPurchase
            {
                OriginalPurchaseNumber = get.PurchaseNumber,
                ArchivedDate = DateTime.UtcNow,
                FlightId = get.Flight.FlightId,
                UserId = get.UserId,
                FirstName = get.FirstName,
                LastName = get.LastName,
                Email = get.Email,
            };
            _context.ArchivedPurchases.Add(archivePurchased);
            _context.Purchases.Remove(get);
            get.Flight.Aircraft.Capacity += 1;
        }
       await _context.SaveChangesAsync();
    }
    
    
    
}
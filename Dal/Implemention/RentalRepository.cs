using Dal.DataObject;
using Dal.Interfaces;
using Microsoft.EntityFrameworkCore;


public class RentalRepository : IRentalRepository
{
    private readonly General _context;

    public RentalRepository(General context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(Rentals rental)
    {
        var newRental = _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();
        return newRental.Entity.Id;
    }

    // פונקציה לקרוא אובייקט לפי ID
    public async Task<Rentals> ReadByIdAsync(int id)
    {
        try
        {
            var rental = await _context.Rentals
                .Include(r => r.Car)  // כלול את רכב השכירות
                .Include(r => r.User) // כלול את המשתמש
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rental == null)
            {
                Console.WriteLine($"Rental not found with ID: {id}");
            }
            return rental;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching Rental by Id {id}: {ex.Message}");
            throw;
        }
    }

    // פונקציה לקרוא את כל השכירויות
    public async Task<List<Rentals>> ReadAllAsync()
    {
        return await _context.Rentals
            .Include(r => r.Car)
            .Include(r => r.User)
            .ToListAsync();
    }

    public async Task<bool> UpdateAsync(Rentals newItem)
    {
        var existingRental = await _context.Rentals.FindAsync(newItem.Id);
        if (existingRental == null)
        {
            return false; 
        }

        existingRental.CarId = newItem.CarId;
        existingRental.UserId = newItem.UserId;
        existingRental.StartDate = newItem.StartDate;
        existingRental.EndDate = newItem.EndDate;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int code)
    {
        var rental = await _context.Rentals.FindAsync(code);
        if (rental == null)
        {
            return false;
        }

        _context.Rentals.Remove(rental);
        await _context.SaveChangesAsync();
        return true;
    }

}

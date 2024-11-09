using AutoMapper;
using BL.DTO;
using BL.Interfaces;
using Dal.DataObject;
using Dal.Interfaces;

namespace BL.Implementation
{
    public class RentalsService : IRentalsService
    {
        private readonly IRentalRepository rentalsRepository;
        private readonly IMapper mapper;

        public RentalsService(IRentalRepository rentalsRepository, IMapper mapper)
        {
            this.rentalsRepository = rentalsRepository;
            this.mapper = mapper;
        }

        // פונקציה ליצירת שכירות
        public async Task<int> CreateAsync(RentalsDTO rentalDto)
        {
            var rental = mapper.Map<Rentals>(rentalDto);
            return await rentalsRepository.CreateAsync(rental);
        }

        // פונקציה לקרוא שכירות לפי ID
        public async Task<RentalsDTO> ReadByIdAsync(int id)
        {
            var rental = await rentalsRepository.ReadByIdAsync(id);
            return rental != null ? mapper.Map<RentalsDTO>(rental) : null;
        }

        // פונקציה לקרוא את כל השכירויות
        public async Task<List<RentalsDTO>> ReadAllAsync()
        {
            var rentals = await rentalsRepository.ReadAllAsync();
            return mapper.Map<List<RentalsDTO>>(rentals);
        }

        // מימוש הפונקציה UpdateAsync (לעדכן שכירות)
        public async Task<bool> UpdateAsync(RentalsDTO rentalDto)
        {
            var rental = mapper.Map<Rentals>(rentalDto);
            return await rentalsRepository.UpdateAsync(rental);
        }

        // מימוש הפונקציה DeleteAsync (למחוק שכירות)
        public async Task<bool> DeleteAsync(int id)
        {
            return await rentalsRepository.DeleteAsync(id);
        }

        // פונקציה לחישוב המחיר
        public double CalculateRentalPrice(double distance, DateTime startDate, DateTime endDate, bool hasDiscount)
        {
            // חישוב תעריף ליחידת זמן
            double hourlyPrice = PriceDetermination.Avg_price_of_taxi_fare_for_km();
            double dailyPrice = PriceDetermination.Price_per_day();

            // חישוב המחיר לפי המרחק
            double distancePrice = distance * hourlyPrice;

            // חישוב המחיר לפי הזמן (פרש בין startDate ל-endDate)
            double rentalDuration = (endDate - startDate).TotalHours;
            double timePrice = rentalDuration * hourlyPrice;

            // סך כל המחיר
            double totalPrice = distancePrice + timePrice;

            // חישוב הנחה
            if (hasDiscount)
            {
                totalPrice -= totalPrice * PriceDetermination.Discount();
            }

            return totalPrice;
        }
    }
}

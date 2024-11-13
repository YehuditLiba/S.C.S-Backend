using AutoMapper;
using BL.DTO;
using BL;
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

        public async Task<int> CreateAsync(RentalsDTO rentalDto)
        {
            var rental = mapper.Map<Rentals>(rentalDto);
            if (rental.CarId != null)
            {
                var station = await rentalsRepository.GetStationByCarIdAsync(rental.CarId);
                if (station != null && station.IsCenteral == true)
                {
                    rental.Price = CalculateRentalPrice(rentalDto, station);
                }
            }

            return await rentalsRepository.CreateAsync(rental);
        }

        public async Task<RentalsDTO> ReadByIdAsync(int id)
        {
            var rental = await rentalsRepository.ReadByIdAsync(id);
            return rental != null ? mapper.Map<RentalsDTO>(rental) : null;
        }

        public async Task<List<RentalsDTO>> ReadAllAsync()
        {
            var rentals = await rentalsRepository.ReadAllAsync();
            var rentalsDto = mapper.Map<List<RentalsDTO>>(rentals);

            // לוגים להצגת הנתונים לאחר המיפוי ל-DTO
            foreach (var rentalDto in rentalsDto)
            {
                Console.WriteLine($"Car Name: {rentalDto.CarName}");
                Console.WriteLine($"User Name: {rentalDto.UserName}");
            }

            return rentalsDto;
        }

        //  UpdateAsync 
        public async Task<bool> UpdateAsync(RentalsDTO rentalDto)
        {
            var rental = mapper.Map<Rentals>(rentalDto);
            return await rentalsRepository.UpdateAsync(rental);
        }

        //  DeleteAsync (למחוק שכירות)
        public async Task<bool> DeleteAsync(int id)
        {
            return await rentalsRepository.DeleteAsync(id);
        }

        public double CalculateRentalPrice(RentalsDTO rentalDto, Station station)
        {
            var basePrice = PriceDetermination.Price_per_day();
            var farePerKm = PriceDetermination.Avg_price_of_taxi_fare_for_km();
            var discount = PriceDetermination.Discount();

            // אם התחנה היא מרכזית, העלה את המחיר ב-20%
            if (station != null && station.IsCenteral == true)
            {
                basePrice *= 1.2;  // העלאת המחיר ב-20%
            }

            // חישוב המחיר עם הנחה
            double finalPrice = basePrice * (1 - discount);
            return finalPrice;
        }
        public async Task<List<RentalsDTO>> GetRentalsByUserNameAsync(string userName)
        {
            var rentals = await rentalsRepository.GetRentalsByUserNameAsync(userName);
            return rentals != null ? mapper.Map<List<RentalsDTO>>(rentals) : null;
        }
        public double CalculateRentalDurationInHours(int rentalId)
        {
            var rental = rentalsRepository.ReadByIdAsync(rentalId).Result;
            if (rental == null)
            {
                throw new Exception("Rental not found");
            }

            // נוודא ששני התאריכים (StartDate ו-EndDate) אינם null לפני החישוב
            if (rental.StartDate.HasValue && rental.EndDate.HasValue)
            {
                // חישוב ההפרש בין StartDate ל-EndDate בשעות
                TimeSpan rentalDuration = rental.EndDate.Value - rental.StartDate.Value;

                // מחזירים את מספר השעות (TotalHours)
                return rentalDuration.TotalHours;
            }
            else
            {
                // אם אחד מהתאריכים לא קיים, נחזיר 0 או ערך אחר מתאים
                throw new Exception("StartDate or EndDate is missing");
            }
        }


    }
}

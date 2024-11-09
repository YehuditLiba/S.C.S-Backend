using System;

namespace BL.DTO
{
    public class RentalsDTO
    {
        public int CarId { get; set; }
        public int UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public RentalsDTO(int carId, int userId, DateTime? startDate, DateTime? endDate)
        {
            CarId = carId;
            UserId = userId;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}

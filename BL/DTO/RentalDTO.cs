using System;

namespace BL.DTO
{
    public class RentalsDTO
    {
        public int CarId { get; set; }
        public int UserId { get; set; }
        public string CarName { get; set; }
        public string UserName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public RentalsDTO(int carId, int userId, string carName, string userName, DateTime? startDate, DateTime? endDate)
        {
            CarId = carId;
            UserId = userId;
            CarName = carName;
            UserName = userName;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}

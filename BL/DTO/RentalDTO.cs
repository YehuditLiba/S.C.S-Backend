using System;

namespace BL.DTO
{
    public class RentalsDTO
    {
        public int UserId { get; set; }
        public int CarId { get; set; }
        public string CarName { get; set; }
        public string UserName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public RentalsDTO( string carName, string userName, DateTime? startDate, DateTime? endDate)
        {
            CarName = carName;
            UserName = userName;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}

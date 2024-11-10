using BL.DTO;
using Dal.DataObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface IRentalsService:IService<RentalsDTO>
    {
        Task<List<RentalsDTO>> GetRentalsByUserNameAsync(string userName);
        double CalculateRentalDurationInHours(int rentalId);

    }
}

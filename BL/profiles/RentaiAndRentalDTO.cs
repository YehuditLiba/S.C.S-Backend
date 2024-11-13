using AutoMapper;
using BL.DTO;
using Dal.DataObject;

public class RentaiAndRentalDTO : Profile
{
    public RentaiAndRentalDTO()
    {
        // מיפוי מ-Rentals ל-RentalsDTO
        CreateMap<Rentals, RentalsDTO>()
            .ForMember(dest => dest.CarName, opt => opt.MapFrom(src => src.Car.Name))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate));

        // מיפוי מ-RentalsDTO ל-Rentals
        CreateMap<RentalsDTO, Rentals>()
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
            .ForMember(dest => dest.Price, opt => opt.Ignore())  
            .ForMember(dest => dest.Car, opt => opt.Ignore())    //  Car
            .ForMember(dest => dest.User, opt => opt.Ignore());  //  User
    }
}

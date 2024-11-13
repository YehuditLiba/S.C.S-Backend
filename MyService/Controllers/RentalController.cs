using AutoMapper;
using BL.DTO;
using BL.Implementation;
using BL.Interfaces;
using Dal.DalImplements;
using Dal.Implemention;
using Dal.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class RentalController : ControllerBase
{
    private readonly IRentalsService rentalService;
    private readonly IMapper _mapper;
    private readonly IUserRepository userRepository;  // הוספת המשתנה
    private readonly ICarRepository carRepository;

    public RentalController(IRentalsService rentalService, IMapper mapper,
                             IUserRepository userRepository, ICarRepository carRepository)
    {
        this.rentalService = rentalService;
        _mapper = mapper;
        this.userRepository = userRepository;  // אתחול המשתנה
        this.carRepository = carRepository;    // אתחול המשתנה
    }

    [HttpPost]
    public async Task<IActionResult> CreateRentalAsync(RentalsDTO rentalDto)
    {
        var user = await userRepository.GetByNameAsync(rentalDto.UserName);
        if (user == null)
        {
            return BadRequest("User not found.");
        }

        var car = await carRepository.GetByNameAsync(rentalDto.CarName);
        if (car == null)
        {
            return BadRequest("Car not found.");
        }

        rentalDto.UserId = user.Code;  
        rentalDto.CarId = car.Id;

        var result = await rentalService.CreateAsync(rentalDto);
        return Ok(result);

    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetRentalById(int id)
    {
        var rental = await rentalService.ReadByIdAsync(id);
        if (rental == null)
        {
            return NotFound($"Rental not found with ID: {id}");
        }
        return Ok(rental);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRentals()
    {
        var rentals = await rentalService.ReadAllAsync();
        return Ok(rentals);
    }
  
    [HttpGet("byUserName/{userName}")]
    public async Task<IActionResult> GetRentalsByUserName(string userName)
    {
        var rentals = await rentalService.GetRentalsByUserNameAsync(userName);
        if (rentals == null || rentals.Count == 0)
        {
            return NotFound($"No rentals found for user: {userName}");
        }
        return Ok(rentals);
    }
   
    [HttpGet("hours/{id}")]
    public async Task<IActionResult> GetRentalByIdWithDuration(int id)
    {
        var rental = await rentalService.ReadByIdAsync(id);
        if (rental == null)
        {
            return NotFound($"Rental not found with ID: {id}");
        }

        double totalHours = rentalService.CalculateRentalDurationInHours(id);

       
        return Ok(new
        {
            rental,
            TotalHours = totalHours  
        });
    }

}




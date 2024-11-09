using AutoMapper;
using BL.DTO;
using BL.Implementation;
using BL.Interfaces;
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

    public RentalController(IRentalsService rentalService, IMapper mapper)
    {
        this.rentalService = rentalService;
        _mapper = mapper;
    }

    // יצירת שכירות חדשה
    [HttpPost]
    public async Task<IActionResult> CreateRentalAsync(RentalsDTO rentalDto)
    {
        var result = await rentalService.CreateAsync(rentalDto);  // שינית את השם כאן
        return Ok(result);
    }

    // פונקציה לקבלת שכירות לפי ID
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

    // פונקציה לקבלת כל השכירויות
    [HttpGet]
    public async Task<IActionResult> GetAllRentals()
    {
        var rentals = await rentalService.ReadAllAsync();
        return Ok(rentals);
    }

}
   
    


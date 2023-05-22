using BL;
using BL.Implementation;
using BL.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddSingleton<IStationService, StationService>();
//builder.Services.AddSingleton<IUserService, UserService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAppServices();

var provider=builder.Services.BuildServiceProvider();
var configuration=provider.GetRequiredService<IConfiguration>();

var app = builder.Build();
//app.AddSingleton<IStationService, StationService>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(options =>
                options.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin());
app.UseAuthorization();

app.MapControllers();

app.Run();

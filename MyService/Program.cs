using AutoMapper;
using BL;
using BL.Implementation;
using BL.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAppServices();

var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();

// check AutoMapper - ����� ��������� ������
try
{
    var mapper = provider.GetRequiredService<AutoMapper.IMapper>();
    Console.WriteLine("AutoMapper service is successfully retrieved.");

    // ������ �� �������� ������
    Console.WriteLine("Asserting AutoMapper configuration...");
    mapper.ConfigurationProvider.AssertConfigurationIsValid();
    Console.WriteLine("AutoMapper configuration is valid.");
}
catch (AutoMapperConfigurationException ex)
{
    Console.WriteLine($"AutoMapper configuration error: {ex.Message}");
    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
    // ���� ������ �� �� ����� �� �� �������� ��� ��� ��� ����� ��� ������
}
catch (Exception ex)
{
    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
}

var app = builder.Build();

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

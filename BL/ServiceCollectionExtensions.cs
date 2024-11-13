using BL.Implementation;
using BL.Interfaces;
using Dal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.profiles;
using Dal.Interfaces;

namespace BL
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IStationService, StationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRentalsService, RentalsService>();
            services.AddScoped<ICarService, CarService>();
            services.AddRepositories();
        }
    }
}

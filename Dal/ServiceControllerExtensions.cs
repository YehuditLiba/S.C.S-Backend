using Dal.DalImplements;
using Dal.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Dal.DataObject;
using Microsoft.EntityFrameworkCore;
using Dal.Implemention;

namespace Dal
{
    public static class ServiceControllerExtensions
    {
        static string connString;
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStationRepository, StationRepository>();
            services.AddDbContext<General>(options => options.UseSqlServer(connString));
        }
        public static string GetConnectionString(string connStrNameInCnfig)
        {
            if (connString != null)
            {
                return connString;
            }
            string connStr = ConfigurationManager.ConnectionStrings[connStrNameInCnfig].ConnectionString;
            connStr = ReplaceWithCurrentLocation(connStr);
            return connStr;
        }

        private static string ReplaceWithCurrentLocation(string connStr)
        {
            string str = AppDomain.CurrentDomain.BaseDirectory;
            string directryAboveBin = str.Substring(0, str.IndexOf("\\bin"));
            string twoDirectoriesAboveBin = directryAboveBin.Substring(0, directryAboveBin.LastIndexOf("\\"));
            connStr = string.Format(connStr, twoDirectoriesAboveBin);
            return connStr;
        }
    }
}

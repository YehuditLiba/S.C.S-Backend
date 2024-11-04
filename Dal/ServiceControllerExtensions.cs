using Dal.DalImplements;
using Dal.DataObject;
using Dal.Implemention;
using Dal.Interfaces;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceControllerExtensions
{
    static string connString; // הגדרת משתנה סטטי גלובלי

    public static void AddRepositories(this IServiceCollection services)
    {
        connString = GetConnectionString("General"); // השתמש במשתנה הגלובלי

        // בדוק אם connString ריק או null
        if (string.IsNullOrEmpty(connString))
        {
            throw new Exception("Connection string is null or empty.");
        }

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IStationRepository, StationRepository>();
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddDbContext<General>(options => options.UseSqlServer(connString));
    }

    public static string GetConnectionString(string connStrNameInCnfig)
    {
        // הפוך את connString למקומי בשיטה
        string connStr = ConfigurationManager.ConnectionStrings[connStrNameInCnfig]?.ConnectionString;

        // בדוק אם connStr לא null
        if (string.IsNullOrEmpty(connStr))
        {
            throw new Exception($"Connection string '{connStrNameInCnfig}' not found.");
        }

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

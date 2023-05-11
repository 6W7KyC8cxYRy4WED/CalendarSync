using CalendarSync.Entities;
using Microsoft.EntityFrameworkCore;

namespace CalendarSync
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").AddUserSecrets<Program>(true).Build();
                    services.AddDbContextPool<DatabaseContext>(options => options.UseSqlServer(configuration.GetConnectionString("Database")));
                    services.AddHostedService<Worker>();
                })
                .Build();

            host.Run();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shelfie.Core.BoardGameGeek;
using Shelfie.Core.Data;
using Shelfie.Core.Services;
using Shelfie.Infrastructure.BoardGameGeek;
using Shelfie.Infrastructure.Data;

namespace Shelfie
{
    public class Program
    {
        public static void Main(string[] args)
            => CreateHostBuilder(args).Build().Run();

        private static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureDefaults(args)
                .ConfigureServices(services =>
                {
                    services.AddTransient<IShelfieRepository, ShelfieRepository>();
                    services.AddTransient<ShelfieService>();
                    services.AddHostedService<Worker>();

                    services.AddDbContext<ShelfieDbContext>(options =>
                        options.UseSqlServer("Server=localhost;Database=Shelfie;Trusted_Connection=True;TrustServerCertificate=True;"));

                    services.AddHttpClient<IBggApiClient, BggApiClient>(client =>
                        client.BaseAddress = new Uri("https://www.boardgamegeek.com/xmlapi/"));
                });
    }
}

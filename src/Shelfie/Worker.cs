using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shelfie.Core.BoardGameGeek;
using Shelfie.Core.Services;

namespace Shelfie
{
    internal class Worker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public Worker(IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime)
        {
            _serviceProvider = serviceProvider;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await using var scope = _serviceProvider.CreateAsyncScope();

                var shelfieService = scope.ServiceProvider.GetRequiredService<ShelfieService>();
                var bggApiClient = scope.ServiceProvider.GetRequiredService<IBggApiClient>();

                Console.WriteLine("Search for a game: ");
                var gameName = GetGameNameFromUser(cancellationToken);

                Console.WriteLine("Searching BGG API for {gameName}");
                var searchResult = await bggApiClient.Search(gameName!);
                foreach (var game in searchResult.BoardGames ?? Enumerable.Empty<BggBoardGame>())
                {
                    Console.WriteLine($"{game.ObjectId}: {game.Name} ({game.YearPublished})");
                }

                Console.WriteLine("Enter ObjectId of game to import: ");
                var bggObjectId = GetBggObjectIdFromUser(cancellationToken);

                Console.WriteLine($"Importing game {bggObjectId}");
                var gameId = await shelfieService.ImportGame(bggObjectId);

                Console.WriteLine($"Game imported with Id {gameId}");
            }
        }

        private static int GetBggObjectIdFromUser(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var input = Console.ReadLine();
                if (int.TryParse(input, out var objectId))
                {
                    return objectId;
                }

                Console.WriteLine("Invalid input. Please enter a number.");
            }

            return default;
        }

        private static string GetGameNameFromUser(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }

                Console.WriteLine("Invalid input. Please enter a valid game name.");
            }

            return default!;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stopping...");
            _hostApplicationLifetime.StopApplication();
            return Task.CompletedTask;
        }
    }
}

using System.Net;
using Microsoft.EntityFrameworkCore;
using RichardSzalay.MockHttp;
using Shelfie.Core.Data;
using Shelfie.Core.Services;
using Shelfie.Infrastructure.BoardGameGeek;
using Shelfie.Infrastructure.Data;
using Shelfie.Tests.Helpers;
using Shouldly;
using Xunit;

namespace Shelfie.Tests.Services
{
    [Trait("Category", TestCategory.IntegrationTest)]
    public class ShelfieService_IntegrationTests
    {
        private const string BaseAddress = "http://www.example.com";
        private readonly ShelfieService _sut;
        private readonly MockHttpMessageHandler _mockHttpMessageHandler = new();
        private readonly ShelfieDbContext _context;

        public ShelfieService_IntegrationTests()
        {
            _context = new ShelfieDbContext(BuildDbContextOptions());
            _sut = new ShelfieService(
                new ShelfieRepository(_context),
                new BggApiClient(new HttpClient(_mockHttpMessageHandler)
                {
                    BaseAddress = new Uri("http://www.example.com")
                }));
        }

        [Fact, AutoRollback]
        public async Task WhenGameDoesNotExist_ShouldAddGameToDatabase()
        {
            // Arrange
            var bggObjectId = 20605;
            var expected = new BoardGame
            {
                Name = "100 Other Games to Play on a Chessboard",
                YearPublished = 1983,
                BggObjectId = bggObjectId
            };
            var expectedContent = @$"
                <boardgames>
		            <boardgame objectid=""{bggObjectId}"">
			            <name primary=""true"">{expected.Name}</name>			
		                <yearpublished>{expected.YearPublished}</yearpublished>
		            </boardgame>
	            </boardgames>";

            _mockHttpMessageHandler
                .Expect(HttpMethod.Get, $"{BaseAddress}/boardgame/{bggObjectId}")
                .Respond(HttpStatusCode.OK, new StringContent(expectedContent));

            // Act
            var result = await _sut.ImportGame(bggObjectId);

            // Assert
            result.ShouldBeGreaterThan(0);
            var actual = await _context.BoardGames.FindAsync(result);
            actual.ShouldSatisfyAllConditions(
                () => actual.ShouldNotBeNull(),
                () => actual!.BggObjectId.ShouldBe(bggObjectId),
                () => actual!.Name.ShouldBe(expected.Name),
                () => actual!.YearPublished.ShouldBe(expected.YearPublished));
        }

        private static DbContextOptions<ShelfieDbContext> BuildDbContextOptions()
        {
            const string connectionString =
                "Server=localhost;Database=Shelfie;Trusted_Connection=True;TrustServerCertificate=True;";

            var builder = new DbContextOptionsBuilder<ShelfieDbContext>()
                .UseSqlServer(connectionString);

            return builder.Options;
        }
    }
}

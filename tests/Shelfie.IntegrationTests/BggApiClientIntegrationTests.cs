using System.Net;
using RichardSzalay.MockHttp;
using Shelfie.Infrastructure.BGG;
using Shouldly;
using Xunit;

namespace Shelfie.IntegrationTests;

public class BggApiClientIntegrationTests
{
    private const string BaseAddress = "http://www.example.com";

    private readonly BggApiClient _bggApiClient;
    private readonly MockHttpMessageHandler _mockHttpMessageHandler = new();

    public BggApiClientIntegrationTests()
    {
        _bggApiClient = new BggApiClient(new HttpClient(_mockHttpMessageHandler)
        {
            BaseAddress = new Uri(BaseAddress)
        });
    }

    [Fact]
    public async Task Test_Search_ReturnsExpectedResult()
    {
        // Arrange
        const string searchTerm = "Chess";
        const int expectedId = 20605;
        const string expectedName = "100 Other Games to Play on a Chessboard";
        const int expectedYearPublished = 1983;

        var expectedContent = @$"
                <boardgames>
		            <boardgame objectid=""{expectedId}"">
			            <name primary=""true"">{expectedName}</name>			
		                <yearpublished>{expectedYearPublished}</yearpublished>
		            </boardgame>
	            </boardgames>";

        _mockHttpMessageHandler
            .Expect(HttpMethod.Get, $"{BaseAddress}/search?search={searchTerm}")
            .Respond(HttpStatusCode.OK, new StringContent(expectedContent));

        // Act
        var result = await _bggApiClient.Search(searchTerm);

        // Assert
        var game = result.BoardGames!.First();
        game.ShouldSatisfyAllConditions(
            () => game.Id.ShouldBe(expectedId),
            () => game.Name.ShouldBe(expectedName),
            () => game.YearPublished.ShouldBe(expectedYearPublished));
    }
}

using System.Net;
using RichardSzalay.MockHttp;
using Shelfie.Core.BoardGameGeek;
using Shelfie.Infrastructure.BoardGameGeek;
using Shelfie.Tests.Helpers;
using Shouldly;
using Xunit;

namespace Shelfie.Tests.BoardGameGeek;

[Trait("Category", TestCategory.IntegrationTest)]
public class BggApiClient_UnitTests
{
    private const string BaseAddress = "http://www.example.com";

    private readonly BggApiClient _bggApiClient;
    private readonly MockHttpMessageHandler _mockHttpMessageHandler = new();

    public BggApiClient_UnitTests()
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

        var expected = new BggBoardGame
        {
            ObjectId = 20605,
            Name = "100 Other Games to Play on a Chessboard",
            YearPublished = 1983
        };

        var expectedContent = @$"
            <boardgames>
		        <boardgame objectid=""{expected.ObjectId}"">
			        <name primary=""true"">{expected.Name}</name>			
		            <yearpublished>{expected.YearPublished}</yearpublished>
		        </boardgame>
	        </boardgames>";

        _mockHttpMessageHandler
            .Expect(HttpMethod.Get, $"{BaseAddress}/search?search={searchTerm}")
            .Respond(HttpStatusCode.OK, new StringContent(expectedContent));

        // Act
        var result = await _bggApiClient.Search(searchTerm);

        // Assert
        result.BoardGames!.First().ShouldBe(expected);
    }

    [Fact]
    public async Task Test_Search_ThrowsHttpRequestException_WhenResponseIsNotSuccessful()
    {
        // Arrange
        const string searchTerm = "Chess";

        _mockHttpMessageHandler
            .Expect(HttpMethod.Get, $"{BaseAddress}/search?search={searchTerm}")
            .Respond(HttpStatusCode.BadRequest);

        // Act
        var task = _bggApiClient.Search(searchTerm);

        // Assert
        await task.ShouldThrowAsync<HttpRequestException>();
    }
}

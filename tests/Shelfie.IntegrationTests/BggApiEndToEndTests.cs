using System.Net;
using Shelfie.Infrastructure.BGG;
using Shouldly;
using Xunit;

namespace Shelfie.IntegrationTests;

public class BggApiEndToEndTests
{
    private readonly BggApiClient _bggApiClient;

    public BggApiEndToEndTests()
    {
        _bggApiClient = new BggApiClient(new HttpClient
        {
            BaseAddress = new Uri("https://www.boardgamegeek.com/xmlapi/")
        });
    }

    [Fact]
    public async Task Test_Search_ShouldReturnExpectedResult()
    {
        // Arrange
        var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("<game><name>Test Game</name></game>"),
        };

        // Act
        var result = await _bggApiClient.Search("chess");

        // Assert
        result.BoardGames.ShouldNotBeEmpty();
    }
}

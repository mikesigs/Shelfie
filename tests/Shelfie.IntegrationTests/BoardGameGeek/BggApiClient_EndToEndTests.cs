using Shelfie.Infrastructure.BoardGameGeek;
using Shelfie.Tests.Helpers;
using Shouldly;
using Xunit;

namespace Shelfie.Tests.BoardGameGeek;

[Trait("Category", TestCategory.EndToEndTest)]
public class BggApiClient_EndToEndTests
{
    private readonly BggApiClient _bggApiClient;

    public BggApiClient_EndToEndTests()
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

        // Act
        var result = await _bggApiClient.Search("Anachrony");

        // Assert
        var game = result.BoardGames?.FirstOrDefault();

        game.ShouldSatisfyAllConditions(
            () => game.ShouldNotBeNull(),
            () => game!.Name.ShouldNotBeEmpty(),
            () => game!.ObjectId.ShouldBeGreaterThan(default),
            () => game!.YearPublished.ShouldBeGreaterThan(default));
    }
}

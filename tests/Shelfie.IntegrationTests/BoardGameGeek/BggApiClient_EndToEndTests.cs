﻿using Shelfie.Infrastructure.BoardGameGeek;
using Shouldly;
using Xunit;

namespace Shelfie.IntegrationTests.BoardGameGeek;

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
        var result = await _bggApiClient.Search("chess");

        // Assert
        result.BoardGames.ShouldNotBeEmpty();
    }
}
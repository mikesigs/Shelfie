using Microsoft.EntityFrameworkCore;
using Shelfie.Core.Data;
using Shelfie.Infrastructure.Data;
using Shelfie.IntegrationTests.Helpers;
using Shouldly;
using Xunit;

namespace Shelfie.IntegrationTests.Data;

public class ShelfieRepository_IntegrationTests
{
    private readonly ShelfieDbContext _context;
    private readonly ShelfieRepository _sut;

    public ShelfieRepository_IntegrationTests()
    {
        var options = new DbContextOptionsBuilder<ShelfieDbContext>()
            .UseSqlServer("Server=localhost;Database=Shelfie;Trusted_Connection=True;TrustServerCertificate=True;") // Using In-Memory database for testing
            .Options;

        _context = new ShelfieDbContext(options);
        _sut = new ShelfieRepository(_context);
    }

    [Fact, AutoRollback]
    public async Task WhenGameExists_DoesBoardGameExist_ShouldReturnTrue()
    {
        // Arrange
        var entity = new BoardGame
        {
            Name = "Wingspan",
            YearPublished = 2019
        };

        _context.BoardGames.Add(entity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.DoesBoardGameExist(entity.Name);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact, AutoRollback]
    public async Task WhenGameDoesNotExist_DoesBoardGameExist_ShouldReturnFalse()
    {
        // Arrange

        // Act
        var result = await _sut.DoesBoardGameExist("Nonexistent Game");

        // Assert
        result.ShouldBeFalse();
    }

    [Fact, AutoRollback]
    public async Task WhenGameExists_GetBoardGame_ShouldReturnBoardGame()
    {
        // Arrange
        var entity = new BoardGame
        {
            Name = "Wingspan",
            YearPublished = 2019
        };

        _context.BoardGames.Add(entity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetBoardGame(entity.Name);

        // Assert
        result.ShouldSatisfyAllConditions(
            () => result.ShouldNotBeNull(),
            () => result!.Name.ShouldBe(entity.Name),
            () => result!.YearPublished.ShouldBe(entity.YearPublished));
    }
}

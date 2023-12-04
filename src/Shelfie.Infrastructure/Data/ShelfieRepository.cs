using Microsoft.EntityFrameworkCore;
using Shelfie.Core.Data;

namespace Shelfie.Infrastructure.Data
{
    public class ShelfieRepository : IShelfieRepository
    {
        private readonly ShelfieDbContext _context;

        public ShelfieRepository(ShelfieDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DoesBoardGameExist(string name)
        {
            return await _context.BoardGames.AnyAsync(bg => bg.Name == name);
        }

        public async Task<BoardGame?> GetBoardGameByName(string name)
        {
            return await _context.BoardGames.FirstOrDefaultAsync(bg => bg.Name == name);
        }

        public async Task<int> AddBoardGame(BoardGame boardGame)
        {
            var addedGame = await _context.BoardGames.AddAsync(boardGame);
            await _context.SaveChangesAsync();
            return addedGame.Entity.Id;
        }
    }
}

using Shelfie.Core.BoardGameGeek;
using Shelfie.Core.Data;

namespace Shelfie.Core.Services
{
    public class ShelfieService
    {
        private readonly IShelfieRepository _shelfieRepository;
        private readonly IBggApiClient _bggApiClient;

        public ShelfieService(IShelfieRepository shelfieRepository, IBggApiClient bggApiClient)
        {
            _shelfieRepository = shelfieRepository;
            _bggApiClient = bggApiClient;
        }

        public async Task<int> ImportGame(int bggObjectId)
        {
            var apiResult = await _bggApiClient.GetBoardGame(bggObjectId);

            if (apiResult.BoardGames?.Length == 0)
            {
                throw new Exception($"Game not found on BGG for ObjectId: {bggObjectId}");
            }

            var bggBoardGame = apiResult.BoardGames!.First();
            if (!await _shelfieRepository.DoesBoardGameExist(bggBoardGame.Name))
            {
                return await _shelfieRepository.AddBoardGame(new BoardGame
                {
                    Name = bggBoardGame.Name,
                    YearPublished = bggBoardGame.YearPublished,
                    BggObjectId = bggBoardGame.ObjectId
                });
            }
            else
            {
                var existingGame = await _shelfieRepository.GetBoardGameByName(bggBoardGame.Name);
                if (existingGame!.BggObjectId != bggBoardGame.ObjectId)
                {
                    throw new Exception($"Game with name {bggBoardGame.Name} already exists with different BGG ObjectId {existingGame.BggObjectId}");
                }

                return existingGame.Id;
            }
        }
    }
}

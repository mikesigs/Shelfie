namespace Shelfie.Core.Data
{
    public interface IShelfieRepository
    {
        Task<bool> DoesBoardGameExist(string name);
        Task<BoardGame?> GetBoardGameByName(string name);
        Task<int> AddBoardGame(BoardGame boardGame);
    }
}

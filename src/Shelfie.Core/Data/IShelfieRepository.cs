namespace Shelfie.Core.Data
{
    public interface IShelfieRepository
    {
        Task<bool> DoesBoardGameExist(string name);
        Task<BoardGame?> GetBoardGame(string name);
    }
}

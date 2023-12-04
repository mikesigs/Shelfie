namespace Shelfie.Core.BoardGameGeek
{
    public interface IBggApiClient
    {
        Task<BggApiResult> Search(string searchTerm);
        Task<BggApiResult> GetBoardGame(int bggObjectId);
    }
}

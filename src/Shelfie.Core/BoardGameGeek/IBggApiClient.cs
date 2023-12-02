namespace Shelfie.Core.BoardGameGeek
{
    public interface IBggApiClient
    {
        Task<BggSearchResult> Search(string searchTerm);
    }
}

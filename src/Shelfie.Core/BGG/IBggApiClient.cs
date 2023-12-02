namespace Shelfie.Core.BGG
{
    public interface IBggApiClient
    {
        Task<BggSearchResults> Search(string searchText);
    }
}

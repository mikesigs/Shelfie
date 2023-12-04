using System.Xml.Serialization;
using Shelfie.Core.BoardGameGeek;

namespace Shelfie.Infrastructure.BoardGameGeek;

public class BggApiClient : IBggApiClient
{
    private readonly HttpClient _httpClient;

    public BggApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BggApiResult> Search(string searchTerm)
    {
        var response = await _httpClient.GetAsync($"search?search={searchTerm}");
        response.EnsureSuccessStatusCode();

        return await DeserializeXmlResponse(response);
    }

    public async Task<BggApiResult> GetBoardGame(int bggObjectId)
    {
        var response = await _httpClient.GetAsync($"boardgame/{bggObjectId}");
        response.EnsureSuccessStatusCode();

        return await DeserializeXmlResponse(response);
    }

    private static async Task<BggApiResult> DeserializeXmlResponse(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        var serializer = new XmlSerializer(typeof(BggApiResult));

        using var reader = new StringReader(content);
        var result = (BggApiResult)serializer.Deserialize(reader)!;
        return result;
    }
}

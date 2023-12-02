using System.Xml.Serialization;
using Shelfie.Core.BGG;

namespace Shelfie.Infrastructure.BGG;

public class BggApiClient : IBggApiClient
{
    private readonly HttpClient _httpClient;

    public BggApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BggSearchResults> Search(string searchText)
    {
        var response = await _httpClient.GetAsync($"search?search={searchText}");
        response.EnsureSuccessStatusCode();

        return await DeserializeXmlResponse(response);
    }

    private static async Task<BggSearchResults> DeserializeXmlResponse(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        var serializer = new XmlSerializer(typeof(BggSearchResults));

        using var reader = new StringReader(content);
        var result = (BggSearchResults)serializer.Deserialize(reader)!;
        return result;
    }
}

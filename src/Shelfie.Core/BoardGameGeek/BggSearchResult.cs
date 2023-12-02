using System.Xml.Serialization;

namespace Shelfie.Core.BoardGameGeek
{
    [XmlRoot("boardgames")]
    public record BggSearchResult
    {
        [XmlElement("boardgame")]
        public BggBoardGame[]? BoardGames { get; init; }
    }
}

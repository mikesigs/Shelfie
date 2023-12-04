using System.Xml.Serialization;

namespace Shelfie.Core.BoardGameGeek
{
    [XmlRoot("boardgames")]
    public record BggApiResult
    {
        [XmlElement("boardgame")]
        public BggBoardGame[]? BoardGames { get; init; }
    }
}

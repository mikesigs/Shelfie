using System.Xml.Serialization;

namespace Shelfie.Core.BoardGameGeek
{
    public record BggBoardGame
    {
        [XmlAttribute("objectid")]
        public int Id { get; init; }

        [XmlElement("name")]
        public string? Name { get; init; }

        [XmlElement("yearpublished")]
        public int? YearPublished { get; init; }
    }
}

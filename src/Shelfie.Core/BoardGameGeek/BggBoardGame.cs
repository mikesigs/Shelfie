using System.Xml.Serialization;

namespace Shelfie.Core.BoardGameGeek
{
    public record BggBoardGame
    {
        [XmlAttribute("objectid")]
        public int ObjectId { get; init; }

        [XmlElement("name")]
        public string Name { get; init; } = null!;

        [XmlElement("yearpublished")]
        public int YearPublished { get; init; }
    }
}

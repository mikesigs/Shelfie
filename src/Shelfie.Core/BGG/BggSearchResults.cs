using System.Xml.Serialization;

namespace Shelfie.Core.BGG
{
    [XmlRoot("boardgames")]
    public class BggSearchResults
    {
        [XmlElement("boardgame")]
        public BoardGame[]? BoardGames { get; set; }

        public class BoardGame
        {
            [XmlAttribute("objectid")]
            public int Id { get; set; }

            [XmlElement("name")]
            public string? Name { get; set; }

            [XmlElement("yearpublished")]
            public int? YearPublished { get; set; }
        }
    }
}

namespace Shelfie.Core.Data
{
    public class BoardGame
    {
        public int Id { get; set; }
        public string Name { get; init; } = null!;
        public int YearPublished { get; init; }
        public int? BggObjectId { get; set; }
    }
}

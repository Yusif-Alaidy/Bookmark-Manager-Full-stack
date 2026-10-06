namespace Bookmark_Manager.Models
{
    public class Bookmark
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public bool IsFavorite { get; set; }
        public DateTime CreatedAt { get; set; }

        public Category Category { get; set; } = null!;
        public int CategoryId { get; set; }

    }
}

using Bookmark_Manager.Models;

namespace Bookmark_Manager.DTOs
{
    public class UpdateBookMark
    {
        public string? Title { get; set; } = string.Empty;
        public string? Url { get; set; } = string.Empty;
        public string? Notes { get; set; } = string.Empty;
        public bool? IsFavorite { get; set; }

        public int? CategoryId { get; set; }

    }
}

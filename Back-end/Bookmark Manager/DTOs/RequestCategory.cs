using System.ComponentModel.DataAnnotations;

namespace Bookmark_Manager.DTOs
{
    public class RequestCategory
    {
        [Required, StringLength(50, MinimumLength = 2)]
        public string Name {  get; set; } = string.Empty;
    }
}

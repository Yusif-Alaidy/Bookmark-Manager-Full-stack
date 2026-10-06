using Bookmark_Manager.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookmark_Manager.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option)
        {
            
        }
        DbSet<Category> categories { get; set; }
        DbSet<Bookmark> bookmarks { get; set; }
    }
}

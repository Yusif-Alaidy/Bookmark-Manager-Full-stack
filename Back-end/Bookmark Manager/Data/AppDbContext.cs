using Microsoft.EntityFrameworkCore;

namespace Bookmark_Manager.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option)
        {
            
        }
    }
}

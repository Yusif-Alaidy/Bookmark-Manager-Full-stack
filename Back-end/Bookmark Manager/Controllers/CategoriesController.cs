using Bookmark_Manager.Data;
using Bookmark_Manager.DTOs;
using Bookmark_Manager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookmark_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        #region Fields & Constructore
        private readonly AppDbContext _context;
        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Get All Category
        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAll() {

            var query = await _context.categories.ToListAsync();
            if (query == null) { 
            return NoContent();
            }
            return Ok(query);
        }
        #endregion

        #region Add Category
        [HttpPost]
        public async Task<IActionResult> CreaateCategory(RequestCategory request)
        {
            if (request == null)
            {
                return BadRequest();
            }
            var cat = new Category { Name = request.Name };
            _context.categories.Add(cat);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        #endregion

        #region Delete Category
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var query = await _context.categories.FindAsync(id);
            if (query == null)
            {
                return NotFound(new {msg = "no category with this id"});
            }
            var hasProducts = await _context.bookmarks.AnyAsync(p => p.CategoryId == id);
            if (hasProducts)
            {
                return BadRequest(new { msg = "Cannot delete a category that has products." });
            }
            _context.categories.Remove(query);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        #endregion
    }
}

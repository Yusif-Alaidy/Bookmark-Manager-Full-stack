using Bookmark_Manager.Data;
using Bookmark_Manager.DTOs;
using Bookmark_Manager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookmark_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookMarkController : ControllerBase
    {
        #region Fields Constructore
        private readonly AppDbContext _context;

        public BookMarkController(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Get All
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bookmark>>> GetAll([FromQuery] int? categoryId, [FromQuery] string? search)
        {
            var bookMarks = _context.bookmarks.AsQueryable();

            if (categoryId.HasValue) {
                bookMarks = bookMarks.Where(e => e.CategoryId == categoryId);
            }

            if (!string.IsNullOrWhiteSpace(search)) {
                bookMarks = bookMarks.Where(e => e.Title.Contains(search));
            }
            return Ok(await bookMarks.ToListAsync());

        }
        #endregion

        #region Get One
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Bookmark>> GetOne(int id) {
            var query = _context.bookmarks.Where(e=>e.Id == id);
            if (!query.Any()) {

                return BadRequest(new {msg="this id not exsist"});
            }

            return Ok(query);
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<ActionResult<Bookmark>> PostBookmark (CreateBookMark request){

            
            var catId = await _context.categories.FindAsync(request.CategoryId);
            if (catId == null)
                return BadRequest(new { msg = "Category does not exist." });

            var newBookMark = new Bookmark()
            {
                Title = request.Title,
                Url = request.Url,
                Notes = request.Notes,
                IsFavorite = request.IsFavorite,
                CategoryId = request.CategoryId,
            };

            _context.bookmarks.Add(newBookMark);
            await _context.SaveChangesAsync();
            return Ok(newBookMark);
        }
        #endregion

        #region Delete
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) { 
            var query = _context.bookmarks.Find(id);
            if (query == null) {
                return BadRequest(new { msg = "this id not exsist" });
            }
            _context.bookmarks.Remove(query);
            _context.SaveChanges();
            return Ok();
        }
        #endregion

        #region Update

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdateBookMark(int id,UpdateBookMark request)
        {

            var query = await _context.bookmarks.FindAsync(id);
            if (query == null)
            {
                return NotFound();
            }
            if (request.CategoryId.HasValue)
            {
                var category = await _context.categories.FindAsync(request.CategoryId);
                if (category == null)
                    return BadRequest(new { msg = "Category does not exist." });

                query.CategoryId = request.CategoryId.Value;
            }

            if (!string.IsNullOrWhiteSpace(request.Title)) query.Title = request.Title!;
            else query.Title = query.Title;
            if (!string.IsNullOrWhiteSpace(request.Url)) query.Url = request.Url!;
            else query.Url = query.Url;
            if (!string.IsNullOrWhiteSpace(request.Notes)) query.Notes = request.Notes!;
            else query.Notes = query.Notes;
            if (request.IsFavorite.HasValue) query.IsFavorite = request.IsFavorite.Value;
            else query.IsFavorite = query.IsFavorite;
            await _context.SaveChangesAsync();

            return Ok();
        }
        #endregion
    }
}

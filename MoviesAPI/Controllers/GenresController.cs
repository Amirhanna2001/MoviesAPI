using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Dtos;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public GenresController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Genre> genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
            return Ok(genres);
        }
        [HttpPost]
        public async Task<IActionResult> Create(GenreDto dto)
        {
            Genre genre = new() { Name = dto.Name};
            await _context.AddAsync(genre);
            _context.SaveChanges();

            return Ok(genre);   
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,[FromBody]GenreDto dto)
        {
            Genre genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);

            if (genre == null)
                return NotFound($"No Genre found by ID = {id}");

            genre.Name = dto.Name;

            _context.SaveChanges();

            return Ok(genre);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Genre genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);

            if (genre == null)
                return NotFound($"No Genre found by ID = {id}");

            _context.Genres.Remove(genre);

            _context.SaveChanges();

            return Ok(genre);

        }
    }
}

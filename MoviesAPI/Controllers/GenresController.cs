using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Dtos;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreServices _genreServices;

        public GenresController(IGenreServices genreServices)
        {
            _genreServices = genreServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Genre> genres =await _genreServices.GetAllGenres();
            return Ok(genres);
        }
        [HttpPost]
        public async Task<IActionResult> Create(GenreDto dto)
        {
            Genre genre = new() { Name = dto.Name};
            await _genreServices.Create(genre);

            return Ok(genre);   
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(byte id,[FromBody]GenreDto dto)
        {
            Genre genre = await _genreServices.GetById(id);

            if (genre == null)
                return NotFound($"No Genre found by ID = {id}");

            genre.Name = dto.Name;

            _genreServices.Update(genre);

            return Ok(genre);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(byte id)
        {
            Genre genre = await _genreServices.GetById(id);

            if (genre == null)
                return NotFound($"No Genre found by ID = {id}");

            _genreServices.Delete(genre);

            return Ok(genre);

        }
    }
}

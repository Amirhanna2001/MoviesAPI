using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private long maxAllowedPosterSize = 1_048_576;
        private List<string> allowedPosterExtentions = new() { "jpg", "png" };

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateMovieDto dto)
        {
            //handle image size 
            //handle extenstions 
            if (dto.Poster.Length > maxAllowedPosterSize)
                return BadRequest("Max allowed size is 1MB !");
            
            if (!allowedPosterExtentions.Contains(Path.GetExtension(dto.Poster.FileName.ToLower())))
                return BadRequest("Only allowed extentions is PNG and JPG !");  
            
            if (await _context.Genres.AnyAsync(g=>g.Id == dto.GenreId))
                return BadRequest("The Genre Id You enterd is not found");



            using MemoryStream dataStream = new ();

            await dto.Poster.CopyToAsync(dataStream);

            Movie movie = new()
            {
                Title = dto.Title,
                Year = dto.Year,
                Rate = dto.Rate,
                StoreLine = dto.StoreLine,
                GenreId = dto.GenreId,
                Poster = dataStream.ToArray()
                
            };

            await _context.Movies.AddAsync(movie);

            _context.SaveChanges();

            return Ok(movie);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Services;
using System;

namespace MoviesAPI.Controllers
{
    //get post put delete
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesServices _moviesServices;
        private readonly IGenreServices _genresServices;
        private long maxAllowedPosterSize = 1_048_576;
        private List<string> allowedPosterExtentions = new() { ".jpg", ".png" };

        public MoviesController(IMoviesServices moviesServices, IGenreServices genresServices)
        {
            _moviesServices = moviesServices;
            _genresServices = genresServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            return Ok(await _moviesServices.GetAllMovies());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            Movie movie =await _moviesServices.GetMovieById(id);

            if (movie == null)
                return NotFound($"Not found movie with id {id}");

            MovieDetailsDto dto = new()
            {
                Id = movie.Id,
                GenreId = movie.GenreId,
                GenreName = movie.Genre.Name,
                Poster = movie.Poster,
                Rate = movie.Rate,
                StoreLine = movie.StoreLine,
                Title = movie.Title,
                Year = movie.Year,
            };

            return Ok(dto);   
        }
        //[HttpGet("GetByGenreId")]
        //public async Task<IActionResult> GetByGenreId(byte genreId)
        //{
        //    return Ok(await _context.Movies
        //        .Where(m=>m.GenreId == genreId)
        //        .OrderByDescending(m => m.Rate)
        //        .Include(m => m.Genre)
        //        .Select(m => new MovieDetailsDto
        //        {
        //            Id = m.Id,
        //            GenreId = m.GenreId,
        //            GenreName = m.Genre.Name,
        //            Poster = m.Poster,
        //            Rate = m.Rate,
        //            StoreLine = m.StoreLine,
        //            Title = m.Title,
        //            Year = m.Year,
        //        })
        //        .ToListAsync());
        //}

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateMovieDto dto)
        {

            if (dto.Poster.Length > maxAllowedPosterSize)
                return BadRequest("Max allowed size is 1MB !");

            if (!allowedPosterExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only allowed extentions is PNG and JPG !");


            if (!await _genresServices.IsGenreExsists(dto.GenreId))
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

            await _moviesServices.Create(movie);

            return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateMovieDto dto)
        {
            Movie movie = await _moviesServices.GetMovieById(id);

            if(movie == null)
                return NotFound($"Not Found movie with id no {id}");

            if (!await _genresServices.IsGenreExsists(dto.GenreId))
                return BadRequest("The Genre Id You enterd is not found");

            if(dto.Poster != null)
            {
                if (dto.Poster.Length > maxAllowedPosterSize)
                    return BadRequest("Poster Size Must Be Less than or equals 1MB ");

                if (!allowedPosterExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only Allowed Extentions .PNG and .JPG ");

                MemoryStream dataStream = new ();
                await dto.Poster.CopyToAsync(dataStream);

                movie.Poster = dataStream.ToArray();
            }
            movie.StoreLine = dto.StoreLine;
            movie.Rate = dto.Rate;
            movie.Title = dto.Title;
            movie.Year = dto.Year;

            _moviesServices.Update(movie);
            return Ok(movie);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Movie movie = await _moviesServices.GetMovieById(id);

            if (movie == null)
                return NotFound($"Not Found movie with id no {id}");

            _moviesServices.Delete(movie);

            return Ok(movie);
        }
    }
}

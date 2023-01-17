﻿namespace MoviesAPI.Services
{
    public interface IMoviesServices
    {
        Task<IEnumerable<Movie>> GetAllMovies();
        Task<Movie> GetMovieById(int id);
        Task<IEnumerable<Movie>> GetByGenreId(byte id);
        Task<Movie> Create(Movie movie);
        Movie Update(Movie movie);
        Movie Delete(Movie movie); 
    }
}

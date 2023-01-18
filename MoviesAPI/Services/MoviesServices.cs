using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Services
{
    public class MoviesServices : IMoviesServices
    {
        private readonly ApplicationDbContext _context;

        public MoviesServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetAllMovies(byte genreId = 0)
        {
            return await _context.Movies
                .Where(x => x.GenreId == genreId || genreId == 0)
                .OrderByDescending(m => m.Rate)
                .Include(m => m.Genre)
                .ToListAsync();
        }
        public async Task<Movie> GetMovieById(int id)
        {
            return await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);
        }
        public async Task<Movie> Create(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            _context.SaveChanges();

            return movie;
        }
        public Movie Update(Movie movie)
        {
            _context.Movies.Update(movie);
            _context.SaveChanges();

            return movie;
        }
        public  Movie Delete(Movie movie)
        {
            _context.Movies.Remove(movie);
            _context.SaveChanges();

            return movie;
        }

       

        public Task<IEnumerable<Movie>> GetByGenreId(byte id)
        {
           throw new NotImplementedException();
        }

        

        
    }
}

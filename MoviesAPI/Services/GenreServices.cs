using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Services
{
    public class GenreServices : IGenreServices
    {
        public readonly ApplicationDbContext _context;

        public GenreServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Genre> Create(Genre genre)
        {
            await _context.AddAsync(genre);
            _context.SaveChanges();

            return genre;
        }

        public Genre Delete(Genre genre)
        {

            _context.Genres.Remove(genre);
            _context.SaveChanges();
            return genre;
        }

        public async Task<IEnumerable<Genre>> GetAllGenres()
            => await _context.Genres.OrderBy(g => g.Name).ToListAsync();



        public async Task<Genre> GetById(byte id) 
            => await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);

        public Genre Update(Genre genre)
        {
            _context.Genres.Update(genre);
            _context.SaveChanges();

            return genre;
        }
    }
}

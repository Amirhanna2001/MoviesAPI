namespace MoviesAPI.Services
{
    public interface IGenreServices
    {
        Task<IEnumerable<Genre>> GetAllGenres();
        Task<Genre> Create(Genre genre);
        Task<Genre> GetById(byte id);
        Genre Update(Genre genre);
        Genre Delete(Genre genre);
        Task<bool> IsGenreExsists(byte id);

    }
}

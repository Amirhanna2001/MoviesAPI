namespace MoviesAPI.Dtos
{
    public class CreateMovieDto:MovieBaseDto
    {
        public IFormFile Poster { get; set; }

    }
}

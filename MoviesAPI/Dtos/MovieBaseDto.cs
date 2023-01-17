namespace MoviesAPI.Dtos
{
    public class MovieBaseDto
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public Double Rate { get; set; }
        public string StoreLine { get; set; }
        public byte GenreId { get; set; }
    }
}

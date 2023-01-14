namespace MoviesAPI.Dtos
{
    public class CreateGenreDto
    {
        [MaxLength(50)]
        public string Name { get; set; }
    }
}

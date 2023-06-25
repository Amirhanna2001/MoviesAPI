namespace MoviesAPI.Models
{
    public class AuthResult
    {
        public string Token { get; set; }
        public bool Succeeded  { get; set; }
        public List<string> Errors { get; set; }
    }
}

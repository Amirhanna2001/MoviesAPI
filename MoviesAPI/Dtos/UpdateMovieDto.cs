﻿namespace MoviesAPI.Dtos
{
    public class UpdateMovieDto:MovieBaseDto
    {
        public IFormFile? Poster { get; set; }
    }
}

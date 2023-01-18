using AutoMapper;

namespace MoviesAPI.Helper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Movie, MovieDetailsDto>();

            CreateMap<CreateMovieDto, Movie>()
                .ForMember(src => src.Poster, opt => opt.Ignore());
        }
    }
}

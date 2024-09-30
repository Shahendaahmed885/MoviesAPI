using AutoMapper;
using MoviesAPI.Dtos;

namespace MoviesAPI.helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, moviedetailsdto>();
            CreateMap<MovieDtos, Movie>()
                .ForMember(src => src.Poster, opt => opt.Ignore());
        }
    }
    
}


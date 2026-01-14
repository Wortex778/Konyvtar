using AutoMapper;
using Konyvtar.DTOs;
using Konyvtar.Models;

namespace Konyvtar.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // DTO -> Entity
            CreateMap<AuthorCreateDTO, Author>();
            CreateMap<BookCreateDTO, Book>();

            // Entity -> DTO
            CreateMap<Author, AuthorReadDTO>();
            CreateMap<Book, BookReadDTO>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name));
        }
    }
}

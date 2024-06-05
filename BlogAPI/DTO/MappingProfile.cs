using AutoMapper;
using BlogAPI.Models;

namespace BlogAPI.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Author, AuthorDTO>().ReverseMap();
            CreateMap<Post, PostDTO>().ReverseMap();
        }
    }
}
//how to use auto mapper in our entites to show only necessary entites at the time post entites like Categories, Authors, Posts
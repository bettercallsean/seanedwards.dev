using AutoMapper;
using MySite.Models;
using MySite.Shared.Dtos;

namespace MySite.AutoMapper;

public class BlogProfile : Profile
{
    public BlogProfile()
    {
        CreateMap<BlogPostDto, BlogPost>();
    }
}

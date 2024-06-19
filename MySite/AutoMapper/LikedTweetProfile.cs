using AutoMapper;
using MySite.Models;
using MySite.Shared.Dtos;

namespace MySite.AutoMapper;

public class LikedTweetProfile : Profile
{
    public LikedTweetProfile()
    {
        CreateMap<LikedTweetDto, LikedTweet>()
            .ForMember(dest => dest.Base64TweetScreenshot,
                mo => mo.MapFrom(src =>
                    $"data:image/png;base64,{Convert.ToBase64String(src.Screenshot!)}"));
    }
}
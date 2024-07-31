using MySite.Shared.Dtos;

namespace MySite.Services.Interfaces;

public interface ITweetService
{
    Task<List<LikedTweetDto>?> GetLikedTweetsAsync();
    Task<List<LikedTweetDto>?> GetLikedTweetsAsync(DateTime tweetDate);
    Task<LikedTweetDto?> GetEarliestLikedTweetAsync();
}
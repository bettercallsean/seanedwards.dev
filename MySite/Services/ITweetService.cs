using MySite.Shared.Dtos;

namespace MySite.Services;

public interface ITweetService
{
    Task<List<LikedTweetDto>?> GetLikedTweetsAsync();
    Task<List<LikedTweetDto>?> GetLikedTweetsAsync(DateTime tweetDate);
    Task<LikedTweetDto?> GetEarliestLikedTweetAsync();
}
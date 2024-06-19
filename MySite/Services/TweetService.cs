using MySite.Shared.Dtos;

namespace MySite.Services;

public class TweetService(HttpClient httpClient, ILogger<TweetService> logger) : ITweetService
{
    private const string BaseUri = "/likedtweets";
    
    public async Task<List<LikedTweetDto>?> GetLikedTweetsAsync()
    {
        logger.LogInformation("Getting all liked tweets");
        return await httpClient.GetFromJsonAsync<List<LikedTweetDto>>($"{BaseUri}");
    }

    public async Task<List<LikedTweetDto>?> GetLikedTweetsAsync(DateTime tweetDate)
    {
        logger.LogInformation("Getting liked tweets for {Date:d}", tweetDate);
        try
        {
            return await httpClient.GetFromJsonAsync<List<LikedTweetDto>>($"{BaseUri}/{tweetDate:dd-MM-yyyy}");
        }
        catch (Exception e)
        {
            logger.LogError("Failed {e}", e);
            throw;
        }
    }

    public async Task<LikedTweetDto?> GetEarliestLikedTweetAsync()
    {
        logger.LogInformation("Getting earliest tweet");
        return await httpClient.GetFromJsonAsync<LikedTweetDto>($"{BaseUri}/earliest");
    }
}
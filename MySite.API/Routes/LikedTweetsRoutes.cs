using MySite.API.Data;
using MySite.API.Data.Dtos;
using MySite.API.Data.Entities;

namespace MySite.API.Routes;

public static class LikedTweetsRoutes
{
    public static void MapLikedTweetsRoutes(this IEndpointRouteBuilder app)
    {
        app.MapPost("/likedtweets", async (LikedTweetsDto dto, MySiteDbContext dbContext) =>
        {
            var tweets = new List<LikedTweet>();
            foreach (var item in dto.Tweets)
            {
                tweets.Add(new LikedTweet
                {
                    TweetLink = item.Key,
                    ScreenshotPath = item.Value,
                    LikedDate = DateTime.Today
                });
            }

            await dbContext.LikedTweets.AddRangeAsync(tweets);

            return await dbContext.SaveChangesAsync() > 0;
        })
        .WithName("AddLikedTweets")
        .WithOpenApi();
    }
}

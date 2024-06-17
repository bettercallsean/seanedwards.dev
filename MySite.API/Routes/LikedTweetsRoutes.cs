using System.Globalization;
using Microsoft.EntityFrameworkCore;
using MySite.API.Data;
using MySite.API.Data.Dtos;
using MySite.API.Data.Entities;

namespace MySite.API.Routes;

public static class LikedTweetsRoutes
{
    private const string TwitterUrl = "https://twitter.com";

    public static void MapLikedTweetsRoutes(this IEndpointRouteBuilder app)
    {
        app.MapPost("/likedtweets", async (LikedTweetsDto dto, MySiteDbContext dbContext) =>
        {
            var tweets = dto.Tweets.Select(item => new LikedTweet
            {
                TweetLink = item.Key, 
                ScreenshotPath = item.Value, 
                LikedDate = DateTime.Today
            }).ToList();

            await dbContext.LikedTweets.AddRangeAsync(tweets);

            return await dbContext.SaveChangesAsync() > 0;
        })
        .WithName("AddLikedTweets")
        .WithOpenApi();
        
        app.MapGet("/likedtweets", async (MySiteDbContext dbContext) =>
        {
            return await dbContext.LikedTweets
                .AsNoTracking()
                .Select(x => new LikedTweetDto
                {
                    TweetLink = $"{TwitterUrl}{x.TweetLink}",
                    ScreenshotPath = x.ScreenshotPath,
                    LikedDate = x.LikedDate
                })
                .ToListAsync();
        })
        .WithName("GetAllLikedTweets")
        .WithOpenApi();
        
        app.MapGet("/likedtweets/{dateString}", async (string dateString, MySiteDbContext dbContext) =>
        {
            if (!DateTime.TryParse(dateString, out var date)) return null;
            
            return await dbContext.LikedTweets
                .AsNoTracking()
                .Where(x => x.LikedDate == date)
                .Select(x => new LikedTweetDto
                {
                    TweetLink = $"{TwitterUrl}{x.TweetLink}",
                    ScreenshotPath = x.ScreenshotPath,
                    LikedDate = x.LikedDate
                })
                .ToListAsync();
        })
        .WithName("GetLikedTweetsOnDate")
        .WithOpenApi();
    }
}

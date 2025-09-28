using System.Globalization;
using Microsoft.EntityFrameworkCore;
using MySite.API.Data;
using MySite.API.Data.Entities;
using MySite.Shared.Dtos;
using MySite.Shared.RouteParameters;

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
                LikedDate = dto.LikedDate
            })
            .ToList();

            tweets.Reverse();

            await dbContext.LikedTweets.AddRangeAsync(tweets);

            return await dbContext.SaveChangesAsync() > 0;
        })
        .WithName("AddLikedTweets")
        .WithOpenApi();

        app.MapGet("/likedtweets", async ([AsParameters] LikedTweetsParameters parameters, MySiteDbContext dbContext) =>
        {
            return await dbContext.LikedTweets
                .AsNoTracking()
                .OrderByDescending(x => x.LikedDate)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .Select(x => new LikedTweetDto
                {
                    TweetLink = $"{TwitterUrl}{x.TweetLink}",
                    Screenshot = string.IsNullOrEmpty(x.ScreenshotPath) ? null : File.ReadAllBytes(x.ScreenshotPath),
                    LikedDate = x.LikedDate
                })
                .ToListAsync();
        })
        .WithName("GetAllLikedTweets")
        .WithOpenApi();

        app.MapGet("/likedtweets/{dateString}", async (string dateString, MySiteDbContext dbContext) =>
        {
            if (!DateTime.TryParseExact(dateString, "dd-MM-yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out var date)) return null;

            date = date.ToUniversalTime();

            var tweets = await dbContext.LikedTweets
                .AsNoTracking()
                .Where(x => x.LikedDate.Date == date.Date)
                .Select(x => new LikedTweetDto
                {
                    TweetLink = $"{TwitterUrl}{x.TweetLink}",
                    Screenshot = string.IsNullOrEmpty(x.ScreenshotPath) ? null : File.ReadAllBytes(x.ScreenshotPath),
                    LikedDate = x.LikedDate
                })
                .ToListAsync();

            tweets.Reverse();

            return tweets;
        })
        .WithName("GetLikedTweetsOnDate")
        .WithOpenApi()
        .CacheOutput(x => x.Expire(new TimeSpan(0, 30, 0)));

        app.MapGet("/likedtweets/earliest", async (MySiteDbContext dbContext) =>
        {
            return await dbContext.LikedTweets
                .AsNoTracking()
                .Select(x => new LikedTweetDto
                {
                    LikedDate = x.LikedDate
                })
                .FirstOrDefaultAsync();
        })
        .WithName("GetEarliestLikedTweet")
        .WithOpenApi()
        .CacheOutput(x => x.Expire(new TimeSpan(0, 30, 0)));
    }
}

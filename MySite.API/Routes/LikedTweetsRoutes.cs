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
        var logger = LoggerFactory.Create(config =>
        {
            config.AddConsole();
        }).CreateLogger("MySite API");

        app.MapPost("/likedtweets", async (LikedTweetsDto dto, MySiteDbContext dbContext) =>
        {
            logger.LogInformation("Storing {LikedTweetCount} liked tweets for {LikedTweetDate}", dto.Tweets.Count, dto.LikedDate);

            try
            {
                var tweets = dto.Tweets.Select(item => new LikedTweet
                {
                    TweetLink = item.Key,
                    ScreenshotPath = item.Value,
                    LikedDate = dto.LikedDate.ToUniversalTime()
                })
                .ToList();

                tweets.Reverse();

                await dbContext.LikedTweets.AddRangeAsync(tweets);

                return await dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to save {LikedTweetCount} liked tweets for {LikedTweetDate}", dto.Tweets.Count, dto.LikedDate);

                return false;
            }

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
            try
            {
                if (!DateTime.TryParseExact(dateString, "dd-MM-yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out var date))
                {
                    logger.LogWarning("Incorrect date string format passed in - {DateString}", dateString);

                    return null;
                }

                date = date.ToUniversalTime();

                logger.LogInformation("Getting liked tweets for {Date}", dateString);

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

                logger.LogInformation("{LikedTweetCount} liked tweets found", tweets.Count);

                return tweets;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to retrieve liked tweets for {DateString}", dateString);

                return null;
            }

        })
        .WithName("GetLikedTweetsOnDate")
        .WithOpenApi()
        .CacheOutput(x => x.Expire(new TimeSpan(0, 30, 0)));

        app.MapGet("/likedtweets/earliest", async (MySiteDbContext dbContext) =>
        {
            logger.LogInformation("Getting earliest tweet date");

            return await dbContext.LikedTweets
                .AsNoTracking()
                .Select(x => new LikedTweetDto
                {
                    LikedDate = x.LikedDate
                })
                .OrderBy(x => x.LikedDate)
                .FirstOrDefaultAsync();
        })
        .WithName("GetEarliestLikedTweet")
        .WithOpenApi()
        .CacheOutput(x => x.Expire(new TimeSpan(0, 30, 0)));
    }
}

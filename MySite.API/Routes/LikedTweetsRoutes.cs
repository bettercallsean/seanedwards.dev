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
                Screenshot = item.Value, 
                LikedDate = DateTime.Today
            }).ToList();

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
                    Screenshot = x.Screenshot,
                    LikedDate = x.LikedDate
                })
                .ToListAsync();
        })
        .WithName("GetAllLikedTweets")
        .WithOpenApi();
        
        app.MapGet("/likedtweets/{dateString}", async (string dateString, MySiteDbContext dbContext) =>
        {
            if (!DateTime.TryParseExact(dateString, "dd-MM-yyyy", CultureInfo.GetCultureInfo("en-GB"), DateTimeStyles.None, out var date)) return null;
            
            return await dbContext.LikedTweets
                .AsNoTracking()
                .Where(x => x.LikedDate == date)
                .Select(x => new LikedTweetDto
                {
                    TweetLink = $"{TwitterUrl}{x.TweetLink}",
                    Screenshot = x.Screenshot,
                    LikedDate = x.LikedDate
                })
                .ToListAsync();
        })
        .WithName("GetLikedTweetsOnDate")
        .WithOpenApi();
        
        app.MapGet("/likedtweets/earliest", async (MySiteDbContext dbContext) =>
            {
                return await dbContext.LikedTweets
                    .AsNoTracking()
                    .Select(x => new LikedTweetDto
                    {
                        TweetLink = $"{TwitterUrl}{x.TweetLink}",
                        Screenshot = x.Screenshot,
                        LikedDate = x.LikedDate
                    })
                    .FirstOrDefaultAsync();
            })
            .WithName("GetEarliestLikedTweet")
            .WithOpenApi();
    }
}

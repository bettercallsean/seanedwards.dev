using Microsoft.EntityFrameworkCore;
using MySite.API.Data;
using MySite.API.Data.Entities;
using MySite.Shared.Dtos;
using MySite.Shared.RouteParameters;
using Slugify;

namespace MySite.API.Routes;

public static class BlogPostsRoutes
{
    public static void MapBlogPostsRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGet("/blogposts", async ([AsParameters] GetAllParameters parameters, MySiteDbContext dbContext) =>
        {
            return await dbContext.BlogPosts
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .Select(x => new BlogPostDto
                {
                    Title = x.Title,
                    UrlSlug = x.UrlSlug,
                    PostedDate = x.PostedDate,
                    Content = x.Content
                })
                .ToListAsync();
        })
        .WithName("GetAllBlogPosts")
        .WithOpenApi();

        app.MapGet("/blogposts/slugs", async ([AsParameters] GetAllParameters parameters, MySiteDbContext dbContext) =>
        {
            return await dbContext.BlogPosts
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .Select(x => x.UrlSlug)
                .ToListAsync();
        })
        .WithName("GetAllBlogPostsSlugs")
        .WithOpenApi();

        app.MapGet("/blogposts/{urlSlug}", async (string urlSlug, MySiteDbContext dbContext) =>
        {
            return await dbContext.BlogPosts
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Where(x => x.UrlSlug == urlSlug)
                .Select(x => new BlogPostDto
                {
                    Title = x.Title,
                    UrlSlug = x.UrlSlug,
                    PostedDate = x.PostedDate,
                    Content = x.Content
                })
                .FirstOrDefaultAsync();
        })
        .WithName("GetBlogPost")
        .WithOpenApi();

        app.MapPost("/blogposts", async (BlogPostDto dto, ISlugHelper slugHelper, MySiteDbContext dbContext) =>
        {
            if (dto.Title is null || dto.Content is null || dto.PostedDate == null) return false;

            await dbContext.BlogPosts.AddAsync(new BlogPost
            {
                Title = dto.Title,
                UrlSlug = slugHelper.GenerateSlug(dto.Title),
                PostedDate = dto.PostedDate.Value,
                Content = dto.Content
            });

            return await dbContext.SaveChangesAsync() > 0;
        })
        .WithName("PostNewBlogPost")
        .WithOpenApi();
    }
}

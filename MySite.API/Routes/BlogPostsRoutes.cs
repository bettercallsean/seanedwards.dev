﻿using Microsoft.EntityFrameworkCore;
using MySite.API.Data;
using MySite.API.Data.Entities;
using MySite.Shared.Dtos;
using MySite.Shared.RouteParameters;

namespace MySite.API.Routes;

public static class BlogPostsRoutes
{
    public static void MapBlogPostsRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGet("/blogposts", async ([AsParameters] GetAllParameters parameters, MySiteDbContext dbContext) =>
        {
            return await dbContext.BlogPosts
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .Select(x => new BlogPostDto
                {
                    Title = x.Title,
                    PostedDate = x.PostedDate,
                    Content = x.Content
                })
                .ToListAsync();
        })
        .WithName("GetAllBlogPosts")
        .WithOpenApi();

        app.MapPost("/blogposts", async (BlogPostDto dto, MySiteDbContext dbContext) =>
        {
            await dbContext.BlogPosts.AddAsync(new BlogPost
            {
                Title = dto.Title,
                PostedDate = dto.PostedDate,
                Content = dto.Content
            });

            return await dbContext.SaveChangesAsync() > 0;
        })
        .WithName("PostNewBlogPost")
        .WithOpenApi();
    }
}
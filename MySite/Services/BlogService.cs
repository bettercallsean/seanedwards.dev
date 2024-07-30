﻿using MySite.Shared.Dtos;

namespace MySite.Services;

public class BlogService(HttpClient httpClient, ILogger<BlogService> logger) : IBlogService
{
    private const string BaseUri = "/blogposts";

    public async Task<List<BlogPostDto>> GetAllBlogPostsAsync()
    {
        logger.LogInformation("Getting all blog posts");
        return await httpClient.GetFromJsonAsync<List<BlogPostDto>>($"{BaseUri}?PageNumber=1&PageSize=5");
    }
}
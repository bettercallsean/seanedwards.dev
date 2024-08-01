﻿using MySite.Services.Interfaces;
using MySite.Shared.Dtos;

namespace MySite.Services;

public class BlogService(HttpClient httpClient, ILogger<BlogService> logger) : IBlogService
{
    private const string BaseUri = "/blogposts";
    private const int PageSize = 5;

    public async Task<List<BlogPostDto>?> GetAllBlogPostsAsync(int pageNumber)
    {
        logger.LogInformation("Getting all blog posts");
        return await httpClient.GetFromJsonAsync<List<BlogPostDto>>($"{BaseUri}?PageNumber={pageNumber}&PageSize={PageSize}");
    }

    public async Task<List<string>?> GetAllBlogPostUrlSlugsAsync(int pageNumber)
    {
        logger.LogInformation("Getting all blog post slugs");
        return await httpClient.GetFromJsonAsync<List<string>>($"{BaseUri}/slugs?PageNumber={pageNumber}&PageSize={PageSize}");
    }

    public async Task<BlogPostDto?> GetBlogPostAsync(string urlSlug)
    {
        logger.LogInformation("Getting blog post {Slug}", urlSlug);
        return await httpClient.GetFromJsonAsync<BlogPostDto>($"{BaseUri}/{urlSlug}");
    }

    public async Task<int> GetBlogPostsCountAsync()
    {
        logger.LogInformation("Getting blog post count");
        return await httpClient.GetFromJsonAsync<int>($"{BaseUri}/count");
    }
}
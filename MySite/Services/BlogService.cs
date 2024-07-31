using MySite.Services.Interfaces;
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

    public async Task<BlogPostDto?> GetBlogPostAsync(string urlSlug)
    {
        logger.LogInformation("Getting blog post {Slug}", urlSlug);
        return await httpClient.GetFromJsonAsync<BlogPostDto>($"{BaseUri}/{urlSlug}");
    }
}

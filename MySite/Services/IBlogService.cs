using MySite.Shared.Dtos;

namespace MySite.Services;

public interface IBlogService
{
    Task<List<BlogPostDto>> GetAllBlogPostsAsync();
    Task<BlogPostDto> GetBlogPostAsync(string urlSlug);
}

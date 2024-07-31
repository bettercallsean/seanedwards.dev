namespace MySite.Shared.Dtos;
public record BlogPostDto
{
    public string? Title { get; set; }
    public string? UrlSlug { get; set; }
    public DateTime? PostedDate { get; set; }
    public string? Content { get; set; }
}

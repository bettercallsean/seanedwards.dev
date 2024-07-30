namespace MySite.Shared.Dtos;
public record BlogPostDto
{
    public string Title { get; set; } = string.Empty;
    public DateTime PostedDate { get; set; }
    public string Content { get; set; } = string.Empty;
}

namespace MySite.Models;

public record BlogPost
{
    public string Title { get; set; } = string.Empty;
    public string UrlSlug { get; set; } = string.Empty;
    public DateTime PostedDate { get; set; }
    public string Content { get; set; } = string.Empty;
}

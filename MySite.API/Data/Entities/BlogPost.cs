namespace MySite.API.Data.Entities;

public class BlogPost
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime PostedDate { get; set; }
    public string Content { get; set; } = string.Empty;
}

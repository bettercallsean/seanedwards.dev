namespace MySite.Shared.Dtos;

public record LikedTweetDto
{
    public string TweetLink { get; set; } = string.Empty;
    public byte[]? Screenshot { get; set; }
    public DateTime LikedDate { get; set; }
}
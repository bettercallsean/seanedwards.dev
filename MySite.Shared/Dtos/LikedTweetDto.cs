namespace MySite.Shared.Dtos;

public record LikedTweetDto
{
    public string? TweetLink { get; set; }
    public byte[]? Screenshot { get; set; }
    public DateTime? LikedDate { get; set; }
}
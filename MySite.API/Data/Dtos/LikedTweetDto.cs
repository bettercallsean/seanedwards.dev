namespace MySite.API.Data.Dtos;

public record LikedTweetDto
{
    public string TweetLink { get; set; } = string.Empty;
    public string ScreenshotPath { get; set; } = string.Empty;
    public DateTime LikedDate { get; set; }
}
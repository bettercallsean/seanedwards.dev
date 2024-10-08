namespace MySite.API.Data.Entities;

public class LikedTweet
{
    public int Id { get; set; }
    public string TweetLink { get; set; } = string.Empty;
    public string ScreenshotPath { get; set; } = string.Empty;
    public DateTime LikedDate { get; set; }
}

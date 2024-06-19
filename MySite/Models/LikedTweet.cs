namespace MySite.Models;

public class LikedTweet
{
    public string TweetLink { get; set; }
    public string Base64TweetScreenshot { get; set; }
    public DateTime LikedDate { get; set; }
}
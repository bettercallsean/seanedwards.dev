namespace MySite.API.Data.Entities;

public class LikedTweet
{
    public int Id { get; set; }
    public string TweetLink { get; set; } = string.Empty;
    public byte[]? Screenshot { get; set; }
    public DateTime LikedDate { get; set; }
}

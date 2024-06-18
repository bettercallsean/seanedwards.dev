namespace MySite.Shared.Dtos;

public record LikedTweetsDto
{
    public Dictionary<string, byte[]> Tweets { get; set; }
}

namespace MySite.API.Data.Dtos;

public record LikedTweetsDto
{
    public Dictionary<string, string> Tweets { get; set; }
}

﻿namespace MySite.Shared.Dtos;

public record LikedTweetsDto
{
    public DateTime LikedDate { get; set; }
    public Dictionary<string, string> Tweets { get; set; }
}

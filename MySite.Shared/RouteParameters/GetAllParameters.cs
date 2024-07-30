namespace MySite.Shared.RouteParameters;

public record GetAllParameters
{
    private const int MaxPageSize = 15;

    private int _pageSize = 5;

    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}
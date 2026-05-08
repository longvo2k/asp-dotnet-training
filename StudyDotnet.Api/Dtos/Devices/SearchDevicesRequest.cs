namespace StudyDotnet.Dtos;

public sealed class SearchDevicesRequest
{
    public string? Keyword { get; set; }
    public string? Supplier { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

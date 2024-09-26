namespace API.Infrastructure.BaseApiDTOs;

public class SearchResponse<T>
{
    public long TotalCount { get; set; }
    public required IEnumerable<T> Items { get; set; }
}
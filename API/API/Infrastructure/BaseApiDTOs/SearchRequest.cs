namespace API.Infrastructure.BaseApiDTOs;

public class SearchRequest : SearchRequestWithoutIds
{
    public HashSet<Guid>? Ids { get; set; }
}
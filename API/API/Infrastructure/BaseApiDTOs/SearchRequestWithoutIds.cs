using System.ComponentModel;

namespace API.Infrastructure.BaseApiDTOs;

public class SearchRequestWithoutIds
{
    [DefaultValue(0)]
    public int Skip { get; set; } = 0;
    [DefaultValue(100)]
    public int Take { get; set; } = 100;
}
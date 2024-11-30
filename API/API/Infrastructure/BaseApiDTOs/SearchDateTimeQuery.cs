namespace API.Infrastructure.BaseApiDTOs;

public class SearchDateTimeQuery
{
    public DateTime From {
        get => from ?? DateTime.MinValue;
        set => from = value;
    }
    public DateTime To {
        get => to ?? DateTime.MaxValue;
        set => to = value;
    }
    
    private DateTime? from;
    private DateTime? to;

    public bool Fit(DateTime? value) => value == null 
        ? from == null 
        : From <= value && value <= To;
}
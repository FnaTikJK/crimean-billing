namespace API.Infrastructure.BaseApiDTOs;

public class SearchFloatQuery
{
    public float From {
        get => from ?? float.MinValue;
        set => from = value;
    }
    public float To {
        get => to ?? float.MaxValue;
        set => to = value;
    }

    private float? from;
    private float? to;

    public bool Fit(float? value) => value == null 
        ? from == null 
        : From <= value && value <= To;
}
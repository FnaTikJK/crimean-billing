namespace API.Infrastructure.BaseApiDTOs;

public class CreateResponse<TKey>
{
    public required TKey Id { get; set; }
    public bool IsCreated { get; set; }
}

public class CreateResponse : CreateResponse<Guid>
{
}
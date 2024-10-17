namespace API.Modules.ServicesModule.Model.DTO;

public class ServiceWithHistoryDTO
{
    public required ServiceDTO Current { get; set; }
    public required List<ServiceHistoryDTO>? History { get; set; }
}

public class ServiceHistoryDTO
{
    public required float Price { get; set; }
    public required float? Amount { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime DeletedAt { get; set; }
}
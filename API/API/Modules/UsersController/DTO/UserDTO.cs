namespace API.Modules.UsersController.DTO;

public class UserDTO
{
    public required Guid UserId { get; set; }
    public required string Email { get; set; }
    public required string Fio { get; set; }
    public required List<AccountDTO> Accounts { get; set; }
}
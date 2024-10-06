namespace API.Modules.AccountsModule.Manager.DTO;

public class ChangeManagerPasswordRequest
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}
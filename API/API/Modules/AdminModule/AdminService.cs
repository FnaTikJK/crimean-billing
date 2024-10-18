using API.Infrastructure;
using API.Modules.AdminModule.DTO;
using API.Modules.CacheModule;

namespace API.Modules.AdminModule;

public interface IAdminService
{
    Result<GetVerificationCodeResponse> GetVerificationCode(GetVerificationCodeRequest request);
    Result<DateTime> MockDateTime(DateTime toMock);
}

public class AdminService : IAdminService
{
    private readonly ICache cache;

    public AdminService(ICache cache)
    {
        this.cache = cache;
    }

    public Result<GetVerificationCodeResponse> GetVerificationCode(GetVerificationCodeRequest request)
    {
        var code = cache.Get(PhoneConverter.ToPhoneWithoutRegMask(request.PhoneNumber));
        return code == null 
            ? Result.BadRequest<GetVerificationCodeResponse>("Нет кода для этого пользователя")
            : Result.Ok(new GetVerificationCodeResponse {VerificationCode = code});
    }

    public Result<DateTime> MockDateTime(DateTime toMock)
    {
        DateTimeProvider.Now = toMock;
        return Result.Ok(DateTimeProvider.Now);
    }
}
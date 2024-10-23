using API.Infrastructure;
using API.Modules.AdminModule.DTO;
using API.Modules.CacheModule;
using API.Modules.InvoiceModule;

namespace API.Modules.AdminModule;

public interface IAdminService
{
    Result<GetVerificationCodeResponse> GetVerificationCode(GetVerificationCodeRequest request);
    Result<DateTime> MockDateTime(DateTime toMock);
    Task<Result<bool>> ForceInvoicesCreation();
}

public class AdminService : IAdminService
{
    private readonly ICache cache;
    private readonly IInvoicesDaemon invoicesDaemon;

    public AdminService(ICache cache, IInvoicesDaemon invoicesDaemon)
    {
        this.cache = cache;
        this.invoicesDaemon = invoicesDaemon;
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

    public Task<Result<bool>> ForceInvoicesCreation()
        => invoicesDaemon.CreateInvoices();
}
using API.Infrastructure;
using API.Modules.AdminModule.DTO;
using API.Modules.CacheModule;
using API.Modules.InvoiceModule;
using API.Modules.PaymentsModule;

namespace API.Modules.AdminModule;

public interface IAdminService
{
    Result<GetVerificationCodeResponse> GetVerificationCode(GetVerificationCodeRequest request);
    Result<DateTime> MockDateTime(DateTime toMock);
    Task<Result<bool>> ForceInvoicesCreation();
    Task<Result<bool>> TryPayInvoices();
}

public class AdminService : IAdminService
{
    private readonly ICache cache;
    private readonly IInvoicesDaemon invoicesDaemon;
    private readonly IPaymentsDaemon paymentsDaemon;

    public AdminService(ICache cache, IInvoicesDaemon invoicesDaemon, IPaymentsDaemon paymentsDaemon)
    {
        this.cache = cache;
        this.invoicesDaemon = invoicesDaemon;
        this.paymentsDaemon = paymentsDaemon;
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

    public Task<Result<bool>> TryPayInvoices()
        => paymentsDaemon.TryPayInvoices();
}
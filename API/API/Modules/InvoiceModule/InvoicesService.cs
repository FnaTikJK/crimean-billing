using API.DAL;
using API.Infrastructure;
using API.Modules.InvoiceModule.DTO;
using API.Modules.InvoiceModule.Model;
using Microsoft.EntityFrameworkCore;

namespace API.Modules.InvoiceModule;

public interface IInvoicesService
{
    public Result<SearchInvoicesResponse> Search(SearchInvoicesRequest request);
}

public class InvoicesService : IInvoicesService
{
    private readonly DataContext db;
    private readonly ILog log;

    private readonly DbSet<InvoiceEntity> invoices;

    public InvoicesService(DataContext db, ILog log)
    {
        this.db = db;
        this.log = log;
        invoices = db.Invoices;
    }

    public Result<SearchInvoicesResponse> Search(SearchInvoicesRequest request)
    {
        var query = invoices.AsNoTracking()
            .Include(e => e.Account)
            .Include(e => e.Tariff)
            .Where(e => e.Account.Id == request.AccountId);

        var totalCount = query.Count();
        return Result.Ok(new SearchInvoicesResponse()
        {
            Items = query.Skip(request.Skip).Take(request.Take).Select(InvoicesMapper.Map).ToList(),
            TotalCount = totalCount
        });
    }
}
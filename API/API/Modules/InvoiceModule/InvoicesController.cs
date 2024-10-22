using API.Modules.InvoiceModule.DTO;
using API.Modules.InvoiceModule.Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Modules.InvoiceModule;

[Route("api/[controller]")]
[ApiController]
public class InvoicesController : ControllerBase
{
    private readonly IInvoicesService invoicesService;

    public InvoicesController(IInvoicesService invoicesService)
    {
        this.invoicesService = invoicesService;
    }

    [HttpPost("Search")]
    public ActionResult<InvoiceDTO> Search(SearchInvoicesRequest request)
    {
        var response = invoicesService.Search(request);
        return response.ActionResult;
    }
}
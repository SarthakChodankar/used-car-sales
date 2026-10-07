using Microsoft.AspNetCore.Mvc;
using UsedCarSales.Application.DTOs.Billing;
using UsedCarSales.Application.Interfaces;

namespace UsedCarSales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly IBillingService _service;

    public SalesController(IBillingService service) => _service = service;

    [HttpPost]
    public async Task<ActionResult<BillDto>> Create(SaleRequest request, CancellationToken ct)
    {
        var bill = await _service.CreateSaleAsync(request, ct);
        return CreatedAtAction(nameof(BillsController.GetById), "Bills", new { id = bill.BillId }, bill);
    }
}
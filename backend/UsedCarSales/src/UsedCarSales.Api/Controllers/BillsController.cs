using Microsoft.AspNetCore.Mvc;
using UsedCarSales.Application.Common;
using UsedCarSales.Application.DTOs.Billing;
using UsedCarSales.Application.Interfaces;

namespace UsedCarSales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillsController : ControllerBase
{
    private readonly IBillingService _service;

    public BillsController(IBillingService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<PagedResult<BillDto>>> GetAll(
        [FromQuery] BillQueryParams query, CancellationToken ct) =>
        Ok(await _service.GetAllAsync(query, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BillDto>> GetById(int id, CancellationToken ct) =>
        Ok(await _service.GetByIdAsync(id, ct));
}
using Microsoft.AspNetCore.Mvc;
using UsedCarSales.Application.Common;
using UsedCarSales.Application.DTOs.Customers;
using UsedCarSales.Application.Interfaces;

namespace UsedCarSales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _service;

    public CustomersController(ICustomerService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<PagedResult<CustomerDto>>> GetAll(
        [FromQuery] CustomerQueryParams query, CancellationToken ct) =>
        Ok(await _service.GetAllAsync(query, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerDto>> GetById(int id, CancellationToken ct) =>
        Ok(await _service.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create(CustomerSaveRequest request, CancellationToken ct)
    {
        var created = await _service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.CustomerId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CustomerSaveRequest request, CancellationToken ct)
    {
        await _service.UpdateAsync(id, request, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _service.DeleteAsync(id, ct);
        return NoContent();
    }
}
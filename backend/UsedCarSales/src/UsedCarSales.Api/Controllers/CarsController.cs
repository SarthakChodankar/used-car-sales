using Microsoft.AspNetCore.Mvc;
using UsedCarSales.Application.Common;
using UsedCarSales.Application.DTOs.Cars;
using UsedCarSales.Application.Interfaces;

namespace UsedCarSales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly ICarService _service;

    public CarsController(ICarService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<PagedResult<CarDto>>> GetAll(
        [FromQuery] CarQueryParams query, CancellationToken ct) =>
        Ok(await _service.GetAllAsync(query, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CarDto>> GetById(int id, CancellationToken ct) =>
        Ok(await _service.GetByIdAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<CarDto>> Create(CarSaveRequest request, CancellationToken ct)
    {
        var created = await _service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.CarId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CarSaveRequest request, CancellationToken ct)
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
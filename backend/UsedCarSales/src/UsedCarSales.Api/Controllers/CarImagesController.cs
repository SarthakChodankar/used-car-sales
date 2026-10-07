using Microsoft.AspNetCore.Mvc;
using UsedCarSales.Application.DTOs.Cars;
using UsedCarSales.Application.Interfaces;

namespace UsedCarSales.Api.Controllers;

[ApiController]
[Route("api/cars/{carId:int}/images")]
public class CarImagesController : ControllerBase
{
    private readonly ICarImageService _service;

    public CarImagesController(ICarImageService service) => _service = service;

    [HttpPost]
    [RequestSizeLimit(6 * 1024 * 1024)]
    public async Task<ActionResult<CarImageDto>> Upload(int carId, IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Please choose an image file.");

        await using var stream = file.OpenReadStream();
        var image = await _service.AddAsync(carId, stream, file.FileName, file.ContentType, file.Length, ct);

        return CreatedAtAction(nameof(CarsController.GetById), "Cars", new { id = carId }, image);
    }

    [HttpPut("{imageId:int}/primary")]
    public async Task<IActionResult> SetPrimary(int carId, int imageId, CancellationToken ct)
    {
        await _service.SetPrimaryAsync(carId, imageId, ct);
        return NoContent();
    }

    [HttpDelete("{imageId:int}")]
    public async Task<IActionResult> Delete(int carId, int imageId, CancellationToken ct)
    {
        await _service.DeleteAsync(carId, imageId, ct);
        return NoContent();
    }
}
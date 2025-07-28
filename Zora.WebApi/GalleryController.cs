using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zora.Core.Features.GalleryServices;

namespace Zora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GalleryController(IGalleryReadService readService, IGalleryWriteService writeService)
    : ControllerBase
{
    [HttpGet("tour/{tourId:long}")]
    public async Task<IActionResult> GetForTour(long tourId, CancellationToken cancellationToken)
    {
        var result = await readService.GetAllForTourAsync(tourId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("destination/{destinationId:long}")]
    public async Task<IActionResult> GetForDestination(
        long destinationId,
        CancellationToken cancellationToken
    )
    {
        var result = await readService.GetAllForDestinationAsync(destinationId, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{tourId:long}")]
    public async Task<IActionResult> Upload(
        long tourId,
        IFormFile? image,
        CancellationToken cancellationToken
    )
    {
        if (image == null || image.Length == 0)
        {
            return BadRequest("Image is required.");
        }

        var result = await writeService.UploadAsync(tourId, image, cancellationToken);
        return CreatedAtAction(nameof(GetForTour), new { tourId }, result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var deleted = await writeService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}

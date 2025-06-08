using Microsoft.AspNetCore.Mvc;
using Zora.Core.TourServices;
using Zora.Core.TourServices.Models;

namespace Zora.WebApi;

[ApiController]
[Route("api/tour/tours")]
public class TourController(ITourWriteService tourWriteService, ITourReadService tourReadService)
    : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Tour>> CreateTour(
        [FromBody] CreateTour createTour,
        CancellationToken cancellationToken
    )
    {
        var result = await tourWriteService.CreateTourAsync(createTour, cancellationToken);
        return CreatedAtAction(nameof(GetTourById), new { tourId = result.Id }, result);
    }

    [HttpGet("{tourId:long}")]
    public async Task<ActionResult<Tour>> GetTourById(
        long tourId,
        CancellationToken cancellationToken
    )
    {
        var tour = await tourReadService.GetTourByIdAsync(tourId, cancellationToken);

        return Ok(tour);
    }

    [HttpGet("user/{userId:long}")]
    public async Task<ActionResult<List<Tour>>> GetAllToursForUser(
        long userId,
        CancellationToken cancellationToken
    )
    {
        var tours = await tourReadService.GetAllToursForUserAsync(userId, cancellationToken);
        return Ok(tours);
    }

    [HttpPut("{tourId:long}")]
    public async Task<ActionResult<Tour>> UpdateTour(
        long tourId,
        [FromBody] UpdateTour updateTour,
        CancellationToken cancellationToken
    )
    {
        var updated = await tourWriteService.UpdateTourAsync(tourId, updateTour, cancellationToken);
        if (updated is null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpDelete("{tourId:long}")]
    public async Task<ActionResult> DeleteTour(long tourId, CancellationToken cancellationToken)
    {
        var success = await tourWriteService.DeleteTourAsync(tourId, cancellationToken);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}

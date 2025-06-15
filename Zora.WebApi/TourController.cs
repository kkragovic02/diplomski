using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zora.Core.Features.TourServices;
using Zora.Core.Features.TourServices.Models;

namespace Zora.WebApi;

[ApiController]
[Route("api/[controller]")]
public class TourController(ITourWriteService tourWriteService, ITourReadService tourReadService)
    : ControllerBase
{
    [HttpPost("tours")]
    public async Task<ActionResult<Tour>> CreateTour(
        [FromBody] CreateTour createTour,
        CancellationToken cancellationToken
    )
    {
        var result = await tourWriteService.CreateAsync(createTour, cancellationToken);
        return CreatedAtAction(nameof(GetTourById), new { tourId = result.Id }, result);
    }

    [HttpGet("tours/{tourId:long}")]
    public async Task<ActionResult<Tour>> GetTourById(
        long tourId,
        CancellationToken cancellationToken
    )
    {
        var tour = await tourReadService.GetByIdAsync(tourId, cancellationToken);

        return Ok(tour);
    }

    [HttpGet("tours/user/{userId:long}")]
    public async Task<ActionResult<List<Tour>>> GetAllToursForUser(
        long userId,
        CancellationToken cancellationToken
    )
    {
        var tours = await tourReadService.GetAllForUserAsync(userId, cancellationToken);
        return Ok(tours);
    }

    [HttpPut("tours/{tourId:long}")]
    public async Task<ActionResult<Tour>> UpdateTour(
        long tourId,
        [FromBody] UpdateTour updateTour,
        CancellationToken cancellationToken
    )
    {
        var updated = await tourWriteService.UpdateAsync(tourId, updateTour, cancellationToken);
        if (updated is null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpDelete("tours/{tourId:long}")]
    public async Task<ActionResult> DeleteTour(long tourId, CancellationToken cancellationToken)
    {
        var success = await tourWriteService.DeleteAsync(tourId, cancellationToken);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}

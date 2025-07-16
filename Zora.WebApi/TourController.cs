using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zora.Core.Features.TourServices;
using Zora.Core.Models;

namespace Zora.WebApi;

[ApiController]
[Route("api/[controller]")]
public class TourController(ITourWriteService tourWriteService, ITourReadService tourReadService)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Tour>>> GetAllTours(CancellationToken cancellationToken)
    {
        var tours = await tourReadService.GetAllAsync(cancellationToken);
        return Ok(tours);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTour(
        CreateTour createTour,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var createdTour = await tourWriteService.CreateAsync(createTour, cancellationToken);
            return Ok(createdTour);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{tourId:long}")]
    public async Task<ActionResult<Tour>> GetTourById(
        long tourId,
        CancellationToken cancellationToken
    )
    {
        var tour = await tourReadService.GetByIdAsync(tourId, cancellationToken);

        return Ok(tour);
    }

    [HttpGet("user/{userId:long}")]
    public async Task<ActionResult<List<Tour>>> GetAllToursForUser(
        long userId,
        CancellationToken cancellationToken
    )
    {
        var tours = await tourReadService.GetAllForUserAsync(userId, cancellationToken);
        return Ok(tours);
    }

    [HttpPut("{tourId:long}")]
    public async Task<ActionResult<Tour>> UpdateTour(
        long tourId,
        [FromBody] UpdateTour updateTour,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var updated = await tourWriteService.UpdateAsync(tourId, updateTour, cancellationToken);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{tourId:long}")]
    public async Task<ActionResult> DeleteTour(long tourId, CancellationToken cancellationToken)
    {
        var success = await tourWriteService.DeleteAsync(tourId, cancellationToken);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("guide-info/{tourId:long}")]
    public async Task<ActionResult<TourWithGuideInfo>> GetTourWithGuideInfo(
        long tourId,
        CancellationToken cancellationToken
    )
    {
        var tour = await tourReadService.GetWithGuideInfoAsync(tourId, cancellationToken);

        if (tour is null)
            return NotFound();

        return Ok(tour);
    }

    [HttpGet("calendar")]
    public async Task<ActionResult<List<TourForCalendar>>> GetToursForCalendar(
        CancellationToken cancellationToken
    )
    {
        var tours = await tourReadService.GetAllForCalendarAsync(cancellationToken);
        return Ok(tours);
    }

    [HttpGet("guide/{guideId}")]
    public async Task<ActionResult<List<Tour>>> GetToursForGuide(
        long guideId,
        CancellationToken cancellationToken
    )
    {
        var tours = await tourReadService.GetAllForGuideAsync(guideId, cancellationToken);
        return Ok(tours);
    }
}

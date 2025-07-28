using Microsoft.AspNetCore.Mvc;
using Zora.Core.Features.AttractionServices;
using Zora.Core.Models;

namespace Zora.WebApi;

[ApiController]
[Route("api/[controller]")]
public class AttractionsController(
    IAttractionReadService attractionReadService,
    IAttractionWriteService attractionWriteService
) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<Attraction>), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<IReadOnlyList<Attraction>>> GetAllAttractionsAsync(
        [FromQuery] long? tourId,
        CancellationToken cancellationToken
    )
    {
        IReadOnlyList<Attraction> attractions;

        if (tourId.HasValue)
        {
            attractions = await attractionReadService.GetByTourIdAsync(
                tourId.Value,
                cancellationToken
            );
        }
        else
        {
            attractions = await attractionReadService.GetAllAsync(cancellationToken);
        }

        return Ok(attractions);
    }

    [HttpPost]
    public async Task<ActionResult<Attraction>> CreateAttractionAsync(
        [FromBody] CreateAttraction createAttraction,
        CancellationToken cancellationToken
    )
    {
        var attraction = await attractionWriteService.CreateAsync(
            createAttraction,
            cancellationToken
        );

        return CreatedAtAction(
            nameof(CreateAttractionAsync),
            new { id = attraction.Id },
            attraction
        );
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<Attraction>> UpdateAttractionAsync(
        [FromRoute] long id,
        [FromBody] UpdateAttraction updatedAttraction,
        CancellationToken cancellationToken
    )
    {
        var updated = await attractionWriteService.UpdateAsync(
            id,
            updatedAttraction,
            cancellationToken
        );

        if (updated is null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteAttractionAsync(
        [FromRoute] long id,
        CancellationToken cancellationToken
    )
    {
        await attractionWriteService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zora.Core.Features.AttractionServices;
using Zora.Core.Features.AttractionServices.Models;

namespace Zora.WebApi;

[ApiController]
[Route("[controller]")]
public class AttractionsController(
    IAttractionReadService readService,
    IAttractionWriteService writeService
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
            attractions = await readService.GetAttractionsByTourIdAsync(
                tourId.Value,
                cancellationToken
            );
        }
        else
        {
            attractions = await readService.GetAllAttractionsAsync(cancellationToken);
        }

        return Ok(attractions);
    }

    [HttpPost]
    public async Task<ActionResult<Attraction>> CreateAttractionAsync(
        [FromBody] CreateAttraction createAttraction,
        CancellationToken cancellationToken
    )
    {
        var attraction = await writeService.CreateAttractionAsync(
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
    public async Task<ActionResult<Attraction>> Update(
        [FromRoute] long id,
        [FromBody] UpdateAttraction updatedAttraction,
        CancellationToken cancellationToken
    )
    {
        var updated = await writeService.UpdateAttractionAsync(
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
    public async Task<IActionResult> Delete(
        [FromRoute] long id,
        CancellationToken cancellationToken
    )
    {
        await writeService.DeleteAttractionAsync(id, cancellationToken);

        return NoContent();
    }
}

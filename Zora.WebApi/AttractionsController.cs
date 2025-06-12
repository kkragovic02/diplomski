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
    public async Task<ActionResult<List<Attraction>>> GetAllAttractionsAsync(
        CancellationToken cancellationToken
    )
    {
        var attractions = await readService.GetAllAttractionsAsync(cancellationToken);
        return Ok(attractions);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Attraction>> GetAttractionByIdAsync(
        [FromRoute] long id,
        CancellationToken cancellationToken
    )
    {
        var attraction = await readService.GetAttractionByIdAsync(id, cancellationToken);

        if (attraction is null)
        {
            return NotFound();
        }

        return Ok(attraction);
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

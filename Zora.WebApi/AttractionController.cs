using Microsoft.AspNetCore.Mvc;
using Zora.Core.AttractionServices;
using Zora.Core.AttractionServices.Models;

namespace Zora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttractionController(
    IAttractionReadService readService,
    IAttractionWriteService writeService
) : ControllerBase
{
    [HttpGet("attraction")]
    public async Task<ActionResult<List<Attraction>>> GetAll(CancellationToken cancellationToken)
    {
        var attractions = await readService.GetAllAttractionsAsync(cancellationToken);
        return Ok(attractions);
    }

    [HttpGet("attractions/{id:int}")]
    public async Task<ActionResult<Attraction>> GetById(int id, CancellationToken cancellationToken)
    {
        var attraction = await readService.GetAttractionByIdAsync(id, cancellationToken);
        return attraction is not null ? Ok(attraction) : NotFound();
    }

    [HttpPost("attractions")]
    public async Task<ActionResult<Attraction>> CreateAttraction(
        [FromBody] CreateAttraction createAttraction,
        CancellationToken cancellationToken
    )
    {
        var attraction = await writeService.CreateAttractionAsync(
            createAttraction,
            cancellationToken
        );
        return CreatedAtAction(nameof(GetById), new { id = attraction.Id }, attraction);
    }

    [HttpPut("attractions/{id:int}")]
    public async Task<ActionResult<Attraction>> Update(
        int id,
        [FromBody] UpdateAttraction updatedAttraction,
        CancellationToken cancellationToken
    )
    {
        var updated = await writeService.UpdateAttractionAsync(
            updatedAttraction,
            id,
            cancellationToken
        );
        return updated is not null ? Ok(updated) : NotFound();
    }

    [HttpDelete("attractions/{id:int}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var deleted = await writeService.DeleteAttractionAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}

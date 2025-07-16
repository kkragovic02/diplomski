using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zora.Core.Features.DestinationServices;
using Zora.Core.Models;

namespace Zora.WebApi;

[ApiController]
[Route("[controller]")]
public class DestinationController(
    IDestinationReadService destinationReadService,
    IDestinationWriteService destinationWriteService
) : ControllerBase
{
    [HttpGet("{destinationId:long}")]
    [ProducesResponseType(typeof(Destination), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Destination>> GetDestinationByIdAsync(
        [FromRoute] long destinationId,
        CancellationToken cancellationToken
    )
    {
        var destination = await destinationReadService.GetByIdAsync(
            destinationId,
            cancellationToken
        );

        if (destination == null)
        {
            return NotFound();
        }

        return destination;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Destination>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Destination>>> GetAllDestinationsAsync(
        CancellationToken cancellationToken
    )
    {
        var destinations = await destinationReadService.GetAllAsync(cancellationToken);
        return Ok(destinations);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Destination), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Destination>> CreateDestinationAsync(
        [FromBody] CreateDestination createDestination,
        CancellationToken cancellationToken
    )
    {
        var destination = await destinationWriteService.CreateAsync(
            createDestination,
            cancellationToken
        );

        return destination;
    }

    [HttpPut("{destinationId:long}")]
    [ProducesResponseType(typeof(Destination), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Destination>> UpdateDestinationAsync(
        [FromRoute] long destinationId,
        [FromBody] UpdateDestination updateDestination,
        CancellationToken cancellationToken
    )
    {
        var updated = await destinationWriteService.UpdateAsync(
            destinationId,
            updateDestination,
            cancellationToken
        );

        if (updated == null)
        {
            return NotFound();
        }

        return updated;
    }

    [HttpDelete("{destinationId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteDestinationAsync(
        [FromRoute] long destinationId,
        CancellationToken cancellationToken
    )
    {
        await destinationWriteService.DeleteAsync(destinationId, cancellationToken);

        return NoContent();
    }
}

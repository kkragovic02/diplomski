using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zora.Core.DestinationServices;
using Zora.Core.DestinationServices.Models;

namespace Zora.WebApi;

[ApiController]
[Route("api/[controller]")]
public class DestinationController(
    IDestinationReadService destinationReadService,
    IDestinationWriteService destinationWriteService
) : ControllerBase
{
    [HttpGet("destinations/{destinationId}")]
    [ProducesResponseType(typeof(Destination), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Destination>> GetDestinationByIdAsync(
        [FromRoute] long destinationId,
        CancellationToken cancellationToken
    )
    {
        var destination = await destinationReadService.GetDestinationByIdAsync(
            destinationId,
            cancellationToken
        );
        if (destination == null)
        {
            return NotFound();
        }

        return destination;
    }

    [HttpPost("destinations")]
    [ProducesResponseType(typeof(Destination), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Destination>> CreateDestinationAsync(
        [FromBody] CreateDestination createDestination,
        CancellationToken cancellationToken
    )
    {
        var destination = await destinationWriteService.CreateDestinationAsync(
            createDestination,
            cancellationToken
        );
        if (destination == null)
        {
            return BadRequest("Invalid destination data.");
        }

        return destination;
    }

    [HttpPut("destinations/{destinationId}")]
    [ProducesResponseType(typeof(Destination), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Destination>> UpdateDestinationAsync(
        [FromRoute] long destinationId,
        [FromBody] UpdateDestination updateDestination,
        CancellationToken cancellationToken
    )
    {
        var updated = await destinationWriteService.UpdateDestinationAsync(
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

    [HttpDelete("destinations/{destinationId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteDestinationAsync(
        [FromRoute] long destinationId,
        CancellationToken cancellationToken
    )
    {
        await destinationWriteService.DeleteDestinationAsync(destinationId, cancellationToken);
        return NoContent();
    }
}

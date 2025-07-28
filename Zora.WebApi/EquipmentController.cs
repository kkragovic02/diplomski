using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zora.Core.Features.EquipmentServices;
using Zora.Core.Models;

namespace Zora.WebApi;

[ApiController]
[Route("api/[controller]")]
public class EquipmentController(
    IEquipmentReadService equipmentReadService,
    IEquipmentWriteService equipmentWriteService
) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<Equipment>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<Equipment>>> GetAllEquipmentAsync(
        [FromQuery] long? tourId,
        CancellationToken cancellationToken
    )
    {
        IReadOnlyList<Equipment> equipment;

        if (tourId.HasValue)
        {
            equipment = await equipmentReadService.GetByTourIdAsync(
                tourId.Value,
                cancellationToken
            );
        }
        else
        {
            equipment = await equipmentReadService.GetAllAsync(cancellationToken);
        }

        return Ok(equipment);
    }

    [HttpGet("{equipmentId}", Name = "GetEquipmentById")]
    [ProducesResponseType(typeof(Equipment), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Equipment>> GetEquipmentByIdAsync(
        [FromRoute] long equipmentId,
        CancellationToken cancellationToken
    )
    {
        var equipment = await equipmentReadService.GetByIdAsync(equipmentId, cancellationToken);

        if (equipment == null)
        {
            return NotFound();
        }

        return equipment;
    }

    [HttpPost]
    public async Task<IActionResult> CreateEquipment(
        [FromBody] CreateEquipment dto,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var result = await equipmentWriteService.CreateAsync(dto, cancellationToken);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message); // 409 - Conflict
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message); // 400 - BadRequest
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Greška na serveru: {ex.Message}");
        }
    }

    [HttpPut("{equipmentId}")]
    [ProducesResponseType(typeof(Equipment), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Equipment>> UpdateEquipmentAsync(
        [FromRoute] long equipmentId,
        [FromBody] UpdateEquipment update,
        CancellationToken cancellationToken
    )
    {
        var updated = await equipmentWriteService.UpdateAsync(
            equipmentId,
            update,
            cancellationToken
        );

        if (updated == null)
        {
            return NotFound();
        }

        return updated;
    }

    [HttpDelete("{equipmentId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteEquipmentAsync(
        [FromRoute] long equipmentId,
        CancellationToken cancellationToken
    )
    {
        await equipmentWriteService.DeleteAsync(equipmentId, cancellationToken);
        return NoContent();
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zora.Core.EquipmentServices;
using Zora.Core.EquipmentServices.Models;

namespace Zora.WebApi;

[ApiController]
[Route("api/[controller]")]
public class EquipmentController(
    IEquipmentReadService equipmentReadService,
    IEquipmentWriteService equipmentWriteService
) : ControllerBase
{
    [HttpGet("equipments")]
    [ProducesResponseType(typeof(IReadOnlyList<Equipment>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<Equipment>>> GetAllEquipmentAsync(
        CancellationToken cancellationToken
    )
    {
        var result = await equipmentReadService.GetAllEquipmentsAsync(cancellationToken);
        return Ok(result.ToList());
    }

    [HttpGet("equipments/{equipmentId}", Name = "GetEquipmentById")]
    [ProducesResponseType(typeof(Equipment), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Equipment>> GetEquipmentByIdAsync(
        [FromRoute] long equipmentId,
        CancellationToken cancellationToken
    )
    {
        var equipment = await equipmentReadService.GetEquipmentByIdAsync(
            equipmentId,
            cancellationToken
        );

        if (equipment == null)
        {
            return NotFound();
        }

        return equipment;
    }

    [HttpPost("equipments")]
    [ProducesResponseType(typeof(Equipment), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Equipment>> CreateEquipmentAsync(
        [FromBody] CreateEquipment createEquipment,
        CancellationToken cancellationToken
    )
    {
        var created = await equipmentWriteService.CreateEquipmentAsync(
            createEquipment,
            cancellationToken
        );
        return CreatedAtAction("GetEquipmentById", new { equipmentId = created.Id }, created);
    }

    [HttpPut("equipments/{equipmentId}")]
    [ProducesResponseType(typeof(Equipment), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Equipment>> UpdateEquipmentAsync(
        [FromRoute] long equipmentId,
        [FromBody] UpdateEquipment update,
        CancellationToken cancellationToken
    )
    {
        var updated = await equipmentWriteService.UpdateEquipmentAsync(
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

    [HttpDelete("equipments/{equipmentId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteEquipmentAsync(
        [FromRoute] long equipmentId,
        CancellationToken cancellationToken
    )
    {
        await equipmentWriteService.DeleteEquipmentAsync(equipmentId, cancellationToken);
        return NoContent();
    }
}

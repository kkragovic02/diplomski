using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zora.Core.Features.EquipmentServices;
using Zora.Core.Features.EquipmentServices.Models;

namespace Zora.WebApi;

[ApiController]
[Route("[controller]")]
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
            equipment = await equipmentReadService.GetEquimpentByTourIdAsync(
                tourId.Value,
                cancellationToken
            );
        }
        else
        {
            equipment = await equipmentReadService.GetAllEquipmentsAsync(cancellationToken);
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

    [HttpPost]
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

    [HttpPut("{equipmentId}")]
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

    [HttpDelete("{equipmentId}")]
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

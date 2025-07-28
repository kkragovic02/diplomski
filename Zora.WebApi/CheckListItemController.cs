using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zora.Core.Features.CheckListItemServices;
using Zora.Core.Features.CheckListItemServices;
using Zora.Core.Models;

namespace Zora.WebApi;

[ApiController]
[Route("api/[controller]")]
public class CheckListController(
    ICheckListReadService readService,
    ICheckListWriteService writeService
) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CheckListItem>>> GetAllAsync(
        CancellationToken cancellationToken
    )
    {
        var items = await readService.GetAllAsync(cancellationToken);
        return Ok(items);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<CheckListItem>> GetByIdAsync(
        long id,
        CancellationToken cancellationToken
    )
    {
        var item = await readService.GetAsync(id, cancellationToken);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<CheckListItem>> CreateAsync(
        [FromBody] CreateCheckListItem item,
        CancellationToken cancellationToken
    )
    {
        var created = await writeService.CreateAsync(item, cancellationToken);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = created.Id }, created);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(id, cancellationToken);
        return result ? NoContent() : NotFound();
    }

    [HttpPost("users/{userId}/tours/{tourId}/equipment/{equipmentId}/check")]
    public async Task<IActionResult> CheckItem(
        [FromRoute] long userId,
        [FromRoute] long tourId,
        [FromRoute] long equipmentId,
        [FromQuery] bool isChecked,
        CancellationToken cancellationToken
    )
    {
        await writeService.UpdateCheckListItemAsync(
            userId,
            tourId,
            equipmentId,
            isChecked,
            cancellationToken
        );
        return Ok(new { success = true });
    }

    [HttpGet("users/{userId}/tours/{tourId}")]
    public async Task<ActionResult<IReadOnlyList<CheckListItem>>> GetByUserAndTourAsync(
        [FromRoute] long userId,
        [FromRoute] long tourId,
        CancellationToken cancellationToken
    )
    {
        var checklist = await readService.GetByUserAndTourAsync(userId, tourId, cancellationToken);
        return Ok(new { checklist });
    }
}

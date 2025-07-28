using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zora.Core.Features.DiaryNoteServices;
using Zora.Core.Models;

namespace Zora.WebApi;

[ApiController]
[Route("api/notes")]
public class DiaryNoteController(
    IDiaryNoteReadService diaryNoteReadService,
    IDiaryNoteWriteService diaryNoteWriteService
) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<DiaryNote>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<DiaryNote>>> GetAllNotesAsync(
        [FromQuery] long? userId,
        CancellationToken cancellationToken
    )
    {
        var notes = await diaryNoteReadService.GetAllAsync(cancellationToken, userId);
        return Ok(notes);
    }

    [HttpGet("{noteId}", Name = "GetNoteById")]
    [ProducesResponseType(typeof(DiaryNote), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DiaryNote>> GetNoteByIdAsync(
        [FromRoute] long noteId,
        CancellationToken cancellationToken
    )
    {
        var note = await diaryNoteReadService.GetByIdAsync(noteId, cancellationToken);
        if (note == null)
            return NotFound();
        return Ok(note);
    }

    [HttpPost]
    [ProducesResponseType(typeof(DiaryNote), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DiaryNote>> CreateNoteAsync(
        [FromBody] CreateDiaryNote createNote,
        CancellationToken cancellationToken
    )
    {
        var note = await diaryNoteWriteService.CreateAsync(createNote, cancellationToken);
        return Ok(note);
    }

    [HttpPut("{noteId}")]
    [ProducesResponseType(typeof(DiaryNote), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DiaryNote>> UpdateNoteAsync(
        [FromRoute] long noteId,
        [FromBody] UpdateDiaryNote updateNote,
        CancellationToken cancellationToken
    )
    {
        var updated = await diaryNoteWriteService.UpdateAsync(
            noteId,
            updateNote,
            cancellationToken
        );
        if (updated == null)
            return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{noteId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteNoteAsync(
        [FromRoute] long noteId,
        CancellationToken cancellationToken
    )
    {
        await diaryNoteWriteService.DeleteAsync(noteId, cancellationToken);
        return NoContent();
    }
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zora.Core.Features.DiaryNoteServices;
using Zora.Core.Features.DiaryNoteServices.Models;

namespace Zora.WebApi;

[ApiController]
[Route("api/[controller]")]
public class DiaryNoteController(
    IDiaryNoteReadService diaryNoteReadService,
    IDiaryNoteWriteService diaryNoteWriteService
) : ControllerBase
{
    [HttpGet("notes")]
    [ProducesResponseType(typeof(IReadOnlyList<DiaryNote>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<DiaryNote>>> GetAllNotesAsync(
        [FromQuery] long? userId,
        CancellationToken cancellationToken
    )
    {
        var notes = await diaryNoteReadService.GetAllNotesAsync(cancellationToken, userId);
        return Ok(notes);
    }

    [HttpGet("notes/{noteId}", Name = "GetNoteById")]
    [ProducesResponseType(typeof(DiaryNote), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DiaryNote>> GetNoteByIdAsync(
        [FromRoute] long noteId,
        CancellationToken cancellationToken
    )
    {
        var note = await diaryNoteReadService.GetNoteByIdAsync(noteId, cancellationToken);
        if (note == null)
            return NotFound();
        return Ok(note);
    }

    [HttpPost("notes")]
    [ProducesResponseType(typeof(DiaryNote), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DiaryNote>> CreateNoteAsync(
        [FromBody] CreateDiaryNote createNote,
        CancellationToken cancellationToken
    )
    {
        var note = await diaryNoteWriteService.CreateNoteAsync(createNote, cancellationToken);
        return CreatedAtAction("GetNoteById", new { noteId = note.Id }, note);
    }

    [HttpPut("notes/{noteId}")]
    [ProducesResponseType(typeof(DiaryNote), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DiaryNote>> UpdateNoteAsync(
        [FromRoute] long noteId,
        [FromBody] UpdateDiaryNote updateNote,
        CancellationToken cancellationToken
    )
    {
        var updated = await diaryNoteWriteService.UpdateNoteAsync(
            noteId,
            updateNote,
            cancellationToken
        );
        if (updated == null)
            return NotFound();
        return Ok(updated);
    }

    [HttpDelete("notes/{noteId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteNoteAsync(
        [FromRoute] long noteId,
        CancellationToken cancellationToken
    )
    {
        await diaryNoteWriteService.DeleteNoteAsync(noteId, cancellationToken);
        return NoContent();
    }
}

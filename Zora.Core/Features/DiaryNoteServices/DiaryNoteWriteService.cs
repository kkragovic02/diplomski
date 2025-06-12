using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.Features.DiaryNoteServices.Models;

namespace Zora.Core.Features.DiaryNoteServices;

internal class DiaryNoteWriteService(ZoraDbContext dbContext) : IDiaryNoteWriteService
{
    public async Task<DiaryNote> CreateNoteAsync(
        CreateDiaryNote createNote,
        CancellationToken cancellationToken
    )
    {
        var model = new DiaryNoteModel
        {
            Title = createNote.Title,
            Content = createNote.Content,
            UserId = createNote.UserId,
            TourId = createNote.TourId,
            CreatedAt = DateTime.UtcNow,
        };

        dbContext.DiaryNotes.Add(model);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DiaryNote(
            model.Id,
            model.Title,
            model.Content,
            model.CreatedAt.DateTime,
            model.UserId,
            model.TourId
        );
    }

    public async Task<DiaryNote?> UpdateNoteAsync(
        long noteId,
        UpdateDiaryNote updateNote,
        CancellationToken cancellationToken
    )
    {
        var note = await dbContext.DiaryNotes.FirstOrDefaultAsync(
            n => n.Id == noteId,
            cancellationToken
        );

        if (note is null)
            return null;

        note.Title = updateNote.Title ?? note.Title;
        note.Content = updateNote.Content ?? note.Content;

        dbContext.DiaryNotes.Update(note);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DiaryNote(
            note.Id,
            note.Title,
            note.Content,
            note.CreatedAt.DateTime,
            note.UserId,
            note.TourId
        );
    }

    public async Task DeleteNoteAsync(long noteId, CancellationToken cancellationToken)
    {
        await dbContext.DiaryNotes.Where(n => n.Id == noteId).ExecuteDeleteAsync(cancellationToken);
    }
}

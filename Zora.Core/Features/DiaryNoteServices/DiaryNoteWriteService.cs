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
        CreateDiaryNote createDiaryNote,
        CancellationToken cancellationToken
    )
    {
        var diaryNoteModel = new DiaryNoteModel
        {
            Title = createDiaryNote.Title,
            Content = createDiaryNote.Content,
            UserId = createDiaryNote.UserId,
            TourId = createDiaryNote.TourId,
            CreatedAt = DateTime.UtcNow,
        };

        dbContext.DiaryNotes.Add(diaryNoteModel);
        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(diaryNoteModel);
    }

    public async Task<DiaryNote?> UpdateNoteAsync(
        long diaryNoteId,
        UpdateDiaryNote updateDiaryNote,
        CancellationToken cancellationToken
    )
    {
        var diaryNote = await dbContext.DiaryNotes.FirstOrDefaultAsync(
            diaryNote => diaryNote.Id == diaryNoteId,
            cancellationToken
        );

        if (diaryNote is null)
            return null;

        diaryNote.Title = updateDiaryNote.Title ?? diaryNote.Title;
        diaryNote.Content = updateDiaryNote.Content ?? diaryNote.Content;

        dbContext.DiaryNotes.Update(diaryNote);
        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(diaryNote);
    }

    public async Task DeleteNoteAsync(long diaryNoteId, CancellationToken cancellationToken)
    {
        await dbContext
            .DiaryNotes.Where(diaryNote => diaryNote.Id == diaryNoteId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    private static DiaryNote MapToDto(DiaryNoteModel diaryNoteModel)
    {
        return new DiaryNote(
            diaryNoteModel.Id,
            diaryNoteModel.Title,
            diaryNoteModel.Content,
            diaryNoteModel.CreatedAt.DateTime,
            diaryNoteModel.UserId,
            diaryNoteModel.TourId
        );
    }
}

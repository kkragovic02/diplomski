using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.Models;

namespace Zora.Core.Features.DiaryNoteServices;

internal class DiaryNoteWriteService(ZoraDbContext dbContext) : IDiaryNoteWriteService
{
    public async Task<DiaryNote> CreateAsync(
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

        return diaryNoteModel.MapToDiaryNote();
    }

    public async Task<DiaryNote?> UpdateAsync(
        long diaryNoteId,
        UpdateDiaryNote updateDiaryNote,
        CancellationToken cancellationToken
    )
    {
        var diaryNoteModel = await dbContext.DiaryNotes.FirstOrDefaultAsync(
            diaryNote => diaryNote.Id == diaryNoteId,
            cancellationToken
        );

        if (diaryNoteModel is null)
        {
            return null;
        }

        diaryNoteModel.Title = updateDiaryNote.Title ?? diaryNoteModel.Title;
        diaryNoteModel.Content = updateDiaryNote.Content ?? diaryNoteModel.Content;

        dbContext.DiaryNotes.Update(diaryNoteModel);
        await dbContext.SaveChangesAsync(cancellationToken);

        return diaryNoteModel.MapToDiaryNote();
    }

    public async Task DeleteAsync(long diaryNoteId, CancellationToken cancellationToken)
    {
        await dbContext
            .DiaryNotes.Where(diaryNote => diaryNote.Id == diaryNoteId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}

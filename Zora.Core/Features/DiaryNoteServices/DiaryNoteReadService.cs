using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Models;

namespace Zora.Core.Features.DiaryNoteServices;

internal class DiaryNoteReadService(ZoraDbContext dbContext) : IDiaryNoteReadService
{
    public async Task<List<DiaryNote>> GetAllAsync(
        CancellationToken cancellationToken,
        long? userId = null
    )
    {
        var diaryNoteModels = await dbContext
            .DiaryNotes.Where(diaryNote => !userId.HasValue || diaryNote.UserId == userId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return diaryNoteModels.Select(diaryNote => diaryNote.MapToDiaryNote()).ToList();
    }

    public async Task<DiaryNote?> GetByIdAsync(
        long diaryNoteId,
        CancellationToken cancellationToken
    )
    {
        var diaryNote = await dbContext
            .DiaryNotes.AsNoTracking()
            .FirstOrDefaultAsync(diaryNote => diaryNote.Id == diaryNoteId, cancellationToken);

        return diaryNote?.MapToDiaryNote();
    }
}

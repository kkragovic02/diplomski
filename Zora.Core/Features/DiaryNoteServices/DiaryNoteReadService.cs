using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.Features.DiaryNoteServices.Models;

namespace Zora.Core.Features.DiaryNoteServices;

internal class DiaryNoteReadService(ZoraDbContext dbContext) : IDiaryNoteReadService
{
    public async Task<List<DiaryNote>> GetAllAsync(
        CancellationToken cancellationToken,
        long? userId = null
    )
    {
        var diaryNotes = await dbContext
            .DiaryNotes.Where(diaryNote => !userId.HasValue || diaryNote.UserId == userId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return diaryNotes.Select(MapToDto).ToList();
    }

    public async Task<DiaryNote?> GetByIdAsync(
        long diaryNoteId,
        CancellationToken cancellationToken
    )
    {
        var diaryNote = await dbContext
            .DiaryNotes.AsNoTracking()
            .FirstOrDefaultAsync(diaryNote => diaryNote.Id == diaryNoteId, cancellationToken);

        return diaryNote is null ? null : MapToDto(diaryNote);
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

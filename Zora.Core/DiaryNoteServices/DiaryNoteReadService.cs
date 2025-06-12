using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.DiaryNoteServices.Models;

namespace Zora.Core.DiaryNoteServices;

internal class DiaryNoteReadService(ZoraDbContext dbContext) : IDiaryNoteReadService
{
    public async Task<List<DiaryNote>> GetAllNotesAsync(
        CancellationToken cancellationToken,
        long? userId = null
    )
    {
        var query = dbContext.DiaryNotes.AsQueryable();

        if (userId.HasValue)
        {
            query = query.Where(n => n.UserId == userId.Value);
        }

        return await query.Select(n => MapToDto(n)).AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<DiaryNote?> GetNoteByIdAsync(long noteId, CancellationToken cancellationToken)
    {
        var note = await dbContext
            .DiaryNotes.AsNoTracking()
            .FirstOrDefaultAsync(n => n.Id == noteId, cancellationToken);

        return note is null ? null : MapToDto(note);
    }

    private static DiaryNote MapToDto(Database.Models.DiaryNoteModel model)
    {
        return new DiaryNote(
            model.Id,
            model.Title,
            model.Content,
            model.CreatedAt.DateTime,
            model.UserId,
            model.TourId
        );
    }
}

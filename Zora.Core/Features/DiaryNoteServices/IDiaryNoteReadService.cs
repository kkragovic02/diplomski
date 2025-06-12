using Zora.Core.Features.DiaryNoteServices.Models;

namespace Zora.Core.Features.DiaryNoteServices;

public interface IDiaryNoteReadService
{
    Task<List<DiaryNote>> GetAllNotesAsync(
        CancellationToken cancellationToken,
        long? userId = null
    );
    Task<DiaryNote?> GetNoteByIdAsync(long noteId, CancellationToken cancellationToken);
}

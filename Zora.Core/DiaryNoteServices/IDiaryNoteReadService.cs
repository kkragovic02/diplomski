using Zora.Core.DiaryNoteServices.Models;

namespace Zora.Core.DiaryNoteServices;

public interface IDiaryNoteReadService
{
    Task<List<DiaryNote>> GetAllNotesAsync(
        CancellationToken cancellationToken,
        long? userId = null
    );
    Task<DiaryNote?> GetNoteByIdAsync(long noteId, CancellationToken cancellationToken);
}

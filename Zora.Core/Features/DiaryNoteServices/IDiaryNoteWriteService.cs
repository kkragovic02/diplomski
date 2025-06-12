using Zora.Core.Features.DiaryNoteServices.Models;

namespace Zora.Core.Features.DiaryNoteServices;

public interface IDiaryNoteWriteService
{
    Task<DiaryNote> CreateNoteAsync(
        CreateDiaryNote createNote,
        CancellationToken cancellationToken
    );
    Task<DiaryNote?> UpdateNoteAsync(
        long noteId,
        UpdateDiaryNote updateNote,
        CancellationToken cancellationToken
    );
    Task DeleteNoteAsync(long noteId, CancellationToken cancellationToken);
}

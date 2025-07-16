using Zora.Core.Models;

namespace Zora.Core.Features.DiaryNoteServices;

public interface IDiaryNoteReadService
{
    Task<List<DiaryNote>> GetAllAsync(CancellationToken cancellationToken, long? userId = null);
    Task<DiaryNote?> GetByIdAsync(long diaryNoteId, CancellationToken cancellationToken);
}

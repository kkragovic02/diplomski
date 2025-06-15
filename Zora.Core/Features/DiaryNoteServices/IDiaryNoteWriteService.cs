using System.Threading;
using System.Threading.Tasks;
using Zora.Core.Features.DiaryNoteServices.Models;

namespace Zora.Core.Features.DiaryNoteServices;

public interface IDiaryNoteWriteService
{
    Task<DiaryNote> CreateAsync(
        CreateDiaryNote createDiaryNote,
        CancellationToken cancellationToken
    );
    Task<DiaryNote?> UpdateAsync(
        long diaryNoteId,
        UpdateDiaryNote updateDiaryNote,
        CancellationToken cancellationToken
    );
    Task DeleteAsync(long diaryNoteId, CancellationToken cancellationToken);
}

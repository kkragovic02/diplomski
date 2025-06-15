using System.Threading;
using System.Threading.Tasks;
using Zora.Core.Features.DiaryNoteServices.Models;

namespace Zora.Core.Features.DiaryNoteServices;

public interface IDiaryNoteWriteService
{
    Task<DiaryNote> CreateNoteAsync(
        CreateDiaryNote createDiaryNote,
        CancellationToken cancellationToken
    );
    Task<DiaryNote?> UpdateNoteAsync(
        long diaryNoteId,
        UpdateDiaryNote updateDiaryNote,
        CancellationToken cancellationToken
    );
    Task DeleteNoteAsync(long diaryNoteId, CancellationToken cancellationToken);
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zora.Core.Features.DiaryNoteServices.Models;

namespace Zora.Core.Features.DiaryNoteServices;

public interface IDiaryNoteReadService
{
    Task<List<DiaryNote>> GetAllAsync(CancellationToken cancellationToken, long? userId = null);
    Task<DiaryNote?> GetByIdAsync(long diaryNoteId, CancellationToken cancellationToken);
}

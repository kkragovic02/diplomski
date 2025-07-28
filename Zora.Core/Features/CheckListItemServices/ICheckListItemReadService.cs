using Zora.Core.Models;

namespace Zora.Core.Features.CheckListItemServices;

public interface ICheckListReadService
{
    Task<IReadOnlyList<CheckListItem>> GetAllAsync(CancellationToken cancellationToken);
    Task<CheckListItem?> GetAsync(long id, CancellationToken cancellationToken);
    Task<IReadOnlyList<UserCheckListItemDto>> GetByUserAndTourAsync(
        long userId,
        long tourId,
        CancellationToken cancellationToken
    );
}

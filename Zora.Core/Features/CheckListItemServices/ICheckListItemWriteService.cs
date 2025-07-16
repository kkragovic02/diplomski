using Zora.Core.Models;

namespace Zora.Core.Features.CheckListItemServices;

public interface ICheckListWriteService
{
    Task<CheckListItem> CreateAsync(CreateCheckListItem item, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken);
    Task UpdateCheckListItemAsync(
        long userId,
        long tourId,
        long equipmentId,
        bool isChecked,
        CancellationToken cancellationToken
    );
}

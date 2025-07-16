using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Models;

namespace Zora.Core.Features.CheckListItemServices;

internal class CheckListReadService(ZoraDbContext dbContext) : ICheckListReadService
{
    public async Task<IReadOnlyList<CheckListItem>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext
            .UserCheckLists.Select(c => c.MapToCheckListItem())
            .ToListAsync(cancellationToken);
    }

    public async Task<CheckListItem?> GetAsync(long id, CancellationToken cancellationToken)
    {
        var item = await dbContext.UserCheckLists.FindAsync(id, cancellationToken);
        return item?.MapToCheckListItem();
    }

    public async Task<IReadOnlyList<CheckListItem>> GetByUserAndTourAsync(
        long userId,
        long tourId,
        CancellationToken cancellationToken
    )
    {
        var items = await dbContext
            .UserCheckLists.Where(item => item.UserId == userId && item.TourId == tourId)
            .Include(i => i.Equipment) // Ako ti treba ime opreme
            .ToListAsync(cancellationToken);

        return items.Select(i => i.MapToCheckListItem()).ToList();
    }
}

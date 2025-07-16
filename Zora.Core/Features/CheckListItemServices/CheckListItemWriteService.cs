using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.Models;

namespace Zora.Core.Features.CheckListItemServices;

internal class CheckListWriteService(ZoraDbContext dbContext) : ICheckListWriteService
{
    public async Task<CheckListItem> CreateAsync(
        CreateCheckListItem item,
        CancellationToken cancellationToken
    )
    {
        var model = new CheckListItemModel()
        {
            IsChecked = item.IsChecked,
            UserId = item.UserId,
            TourId = item.TourId,
            EquipmentId = item.EquipmentId,
        };

        dbContext.UserCheckLists.Add(model);
        await dbContext.SaveChangesAsync(cancellationToken);

        return model.MapToCheckListItem();
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var item = await dbContext.UserCheckLists.FindAsync(new object[] { id }, cancellationToken);
        if (item == null)
            return false;

        dbContext.UserCheckLists.Remove(item);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task UpdateCheckListItemAsync(
        long userId,
        long tourId,
        long equipmentId,
        bool isChecked,
        CancellationToken cancellationToken
    )
    {
        var checkListItemModel = await dbContext.UserCheckLists.FirstOrDefaultAsync(
            i => i.UserId == userId && i.TourId == tourId && i.EquipmentId == equipmentId,
            cancellationToken
        );

        if (checkListItemModel == null)
        {
            dbContext.UserCheckLists.Add(
                new CheckListItemModel
                {
                    UserId = userId,
                    TourId = tourId,
                    EquipmentId = equipmentId,
                    IsChecked = isChecked,
                }
            );
        }
        else
        {
            checkListItemModel.IsChecked = isChecked;
            dbContext.UserCheckLists.Update(checkListItemModel);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

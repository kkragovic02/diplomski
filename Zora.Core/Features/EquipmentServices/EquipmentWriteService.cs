using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.Models;

namespace Zora.Core.Features.EquipmentServices;

internal class EquipmentWriteService(ZoraDbContext dbContext) : IEquipmentWriteService
{
    public async Task<Equipment> CreateAsync(
        CreateEquipment createEquipment,
        CancellationToken cancellationToken
    )
    {
        var equipmentModel = new EquipmentModel { Name = createEquipment.Name };

        dbContext.Equipments.Add(equipmentModel);
        await dbContext.SaveChangesAsync(cancellationToken);

        return equipmentModel.MapToEquipment();
    }

    public async Task<Equipment?> UpdateAsync(
        long equipmentId,
        UpdateEquipment updateEquipment,
        CancellationToken cancellationToken
    )
    {
        var equipmentModel = await dbContext.Equipments.FirstOrDefaultAsync(
            equipment => equipment.Id == equipmentId,
            cancellationToken
        );

        if (equipmentModel == null)
            return null;

        equipmentModel.Name = updateEquipment.Name ?? equipmentModel.Name;

        dbContext.Equipments.Update(equipmentModel);
        await dbContext.SaveChangesAsync(cancellationToken);

        return equipmentModel.MapToEquipment();
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        await dbContext
            .Equipments.Where(equipment => equipment.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}

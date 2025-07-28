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
        if (string.IsNullOrWhiteSpace(createEquipment.Name))
        {
            throw new ArgumentException("Naziv opreme je obavezan.");
        }

        var exists = await dbContext.Equipments.AnyAsync(
            e => e.Name == createEquipment.Name,
            cancellationToken
        );

        if (exists)
        {
            throw new InvalidOperationException(
                $"Oprema sa nazivom '{createEquipment.Name}' već postoji."
            );
        }

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
        var equipment = await dbContext
            .Equipments.Include(e => e.Tours)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (equipment == null)
            return;

        equipment.Tours.Clear();

        dbContext.Equipments.Remove(equipment);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

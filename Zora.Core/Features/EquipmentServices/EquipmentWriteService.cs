using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.Features.EquipmentServices.Models;

namespace Zora.Core.Features.EquipmentServices;

internal class EquipmentWriteService(ZoraDbContext dbContext) : IEquipmentWriteService
{
    public async Task<Equipment> CreateEquipmentAsync(
        CreateEquipment createEquipment,
        CancellationToken cancellationToken
    )
    {
        var equipment = new EquipmentModel { Name = createEquipment.Name };

        dbContext.Equipments.Add(equipment);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new Equipment(equipment.Id, equipment.Name);
    }

    public async Task<Equipment?> UpdateEquipmentAsync(
        long equipmentId,
        UpdateEquipment updateEquipment,
        CancellationToken cancellationToken
    )
    {
        var equipment = await dbContext.Equipments.FirstOrDefaultAsync(
            equipment => equipment.Id == equipmentId,
            cancellationToken
        );

        if (equipment == null)
            return null;

        equipment.Name = updateEquipment.Name ?? equipment.Name;

        dbContext.Equipments.Update(equipment);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new Equipment(equipment.Id, equipment.Name);
    }

    public async Task DeleteEquipmentAsync(long id, CancellationToken cancellationToken)
    {
        await dbContext
            .Equipments.Where(equipment => equipment.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}

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
        CreateEquipment equipmentDto,
        CancellationToken cancellationToken
    )
    {
        var equipment = new EquipmentModel { Name = equipmentDto.Name };

        dbContext.Equipments.Add(equipment);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new Equipment(equipment.Id, equipment.Name);
    }

    public async Task<Equipment?> UpdateEquipmentAsync(
        long id,
        UpdateEquipment updateDto,
        CancellationToken cancellationToken
    )
    {
        var equipment = await dbContext.Equipments.FirstOrDefaultAsync(
            e => e.Id == id,
            cancellationToken
        );

        if (equipment == null)
            return null;

        equipment.Name = updateDto.Name ?? equipment.Name;

        dbContext.Equipments.Update(equipment);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new Equipment(equipment.Id, equipment.Name);
    }

    public async Task DeleteEquipmentAsync(long id, CancellationToken cancellationToken)
    {
        await dbContext.Equipments.Where(e => e.Id == id).ExecuteDeleteAsync(cancellationToken);
    }
}

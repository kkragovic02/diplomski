using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Features.EquipmentServices.Models;

namespace Zora.Core.Features.EquipmentServices;

internal class EquipmentReadService(ZoraDbContext dbContext) : IEquipmentReadService
{
    public async Task<IEnumerable<Equipment>> GetAllEquipmentsAsync(
        CancellationToken cancellationToken
    )
    {
        return await dbContext
            .Equipments.Select(e => new Equipment(e.Id, e.Name))
            .ToListAsync(cancellationToken);
    }

    public async Task<Equipment?> GetEquipmentByIdAsync(
        long id,
        CancellationToken cancellationToken
    )
    {
        var equipment = await dbContext.Equipments.FirstOrDefaultAsync(
            e => e.Id == id,
            cancellationToken
        );

        return equipment == null ? null : new Equipment(equipment.Id, equipment.Name);
    }
}

using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Models;

namespace Zora.Core.Features.EquipmentServices;

internal class EquipmentReadService(ZoraDbContext dbContext) : IEquipmentReadService
{
    public async Task<IReadOnlyList<Equipment>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext
            .Equipments.Select(equipment => equipment.MapToEquipment())
            .ToListAsync(cancellationToken);
    }

    public async Task<Equipment?> GetByIdAsync(
        long equipmentId,
        CancellationToken cancellationToken
    )
    {
        var equipmentModel = await dbContext.Equipments.FirstOrDefaultAsync(
            equipment => equipment.Id == equipmentId,
            cancellationToken
        );

        return equipmentModel?.MapToEquipment();
    }

    public async Task<IReadOnlyList<Equipment>> GetByTourIdAsync(
        long tourId,
        CancellationToken cancellationToken
    )
    {
        var equipmentModels = await dbContext
            .Equipments.Include(equipment => equipment.Tours)
            .Where(equipment => equipment.Tours.Any(tour => tour.Id == tourId))
            .ToListAsync(cancellationToken);

        return equipmentModels.ConvertAll(equipment => equipment.MapToEquipment());
    }
}

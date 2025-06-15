using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.Features.EquipmentServices.Models;

namespace Zora.Core.Features.EquipmentServices;

internal class EquipmentReadService(ZoraDbContext dbContext) : IEquipmentReadService
{
    public async Task<IReadOnlyList<Equipment>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext
            .Equipments.Select(equipment => MapToEquipment(equipment))
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

        return equipmentModel == null ? null : MapToEquipment(equipmentModel);
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

        return equipmentModels.ConvertAll(MapToEquipment);
    }

    private static Equipment MapToEquipment(EquipmentModel equipmentModel)
    {
        return new Equipment(equipmentModel.Id, equipmentModel.Name);
    }
}

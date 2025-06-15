using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zora.Core.Features.EquipmentServices.Models;

namespace Zora.Core.Features.EquipmentServices;

public interface IEquipmentReadService
{
    Task<IReadOnlyList<Equipment>> GetAllAsync(CancellationToken cancellationToken);

    Task<Equipment?> GetByIdAsync(long equipmentId, CancellationToken cancellationToken);

    Task<IReadOnlyList<Equipment>> GetByTourIdAsync(
        long tourId,
        CancellationToken cancellationToken
    );
}

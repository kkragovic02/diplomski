using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zora.Core.Features.EquipmentServices.Models;

namespace Zora.Core.Features.EquipmentServices;

public interface IEquipmentReadService
{
    Task<IReadOnlyList<Equipment>> GetAllEquipmentsAsync(CancellationToken cancellationToken);

    Task<Equipment?> GetEquipmentByIdAsync(long id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Equipment>> GetEquimpentByTourIdAsync(
        long tourId,
        CancellationToken cancellationToken
    );
}

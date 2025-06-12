using Zora.Core.Features.EquipmentServices.Models;

namespace Zora.Core.Features.EquipmentServices;

public interface IEquipmentReadService
{
    Task<IEnumerable<Equipment>> GetAllEquipmentsAsync(CancellationToken cancellationToken);
    Task<Equipment?> GetEquipmentByIdAsync(long id, CancellationToken cancellationToken);
}

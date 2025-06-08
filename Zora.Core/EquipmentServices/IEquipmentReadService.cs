using Zora.Core.EquipmentServices.Models;

namespace Zora.Core.EquipmentServices;

public interface IEquipmentReadService
{
    Task<IEnumerable<Equipment>> GetAllEquipmentsAsync(CancellationToken cancellationToken);
    Task<Equipment?> GetEquipmentByIdAsync(long id, CancellationToken cancellationToken);
}

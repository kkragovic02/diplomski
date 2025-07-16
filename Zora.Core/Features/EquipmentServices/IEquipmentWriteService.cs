using Zora.Core.Models;

namespace Zora.Core.Features.EquipmentServices;

public interface IEquipmentWriteService
{
    Task<Equipment> CreateAsync(
        CreateEquipment createEquipment,
        CancellationToken cancellationToken
    );
    Task<Equipment?> UpdateAsync(
        long equipmentId,
        UpdateEquipment updateEquipment,
        CancellationToken cancellationToken
    );
    Task DeleteAsync(long id, CancellationToken cancellationToken);
}

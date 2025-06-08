using Zora.Core.EquipmentServices.Models;

namespace Zora.Core.EquipmentServices;

public interface IEquipmentWriteService
{
    Task<Equipment> CreateEquipmentAsync(
        CreateEquipment equipment,
        CancellationToken cancellationToken
    );
    Task<Equipment?> UpdateEquipmentAsync(
        long id,
        UpdateEquipment equipment,
        CancellationToken cancellationToken
    );
    Task DeleteEquipmentAsync(long id, CancellationToken cancellationToken);
}

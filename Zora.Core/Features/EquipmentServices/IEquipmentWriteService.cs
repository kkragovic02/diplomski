using System.Threading;
using System.Threading.Tasks;
using Zora.Core.Features.EquipmentServices.Models;

namespace Zora.Core.Features.EquipmentServices;

public interface IEquipmentWriteService
{
    Task<Equipment> CreateEquipmentAsync(
        CreateEquipment createEquipment,
        CancellationToken cancellationToken
    );
    Task<Equipment?> UpdateEquipmentAsync(
        long equipmentId,
        UpdateEquipment updateEquipment,
        CancellationToken cancellationToken
    );
    Task DeleteEquipmentAsync(long id, CancellationToken cancellationToken);
}

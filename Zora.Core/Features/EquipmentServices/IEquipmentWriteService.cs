using System.Threading;
using System.Threading.Tasks;
using Zora.Core.Features.EquipmentServices.Models;

namespace Zora.Core.Features.EquipmentServices;

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

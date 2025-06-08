using Zora.Core.DestinationServices.Models;

namespace Zora.Core.DestinationServices;

public interface IDestinationWriteService
{
    Task<Destination?> CreateDestinationAsync(
        CreateDestination createDestination,
        CancellationToken cancellationToken
    );
    Task<Destination?> UpdateDestinationAsync(
        long destinationId,
        UpdateDestination updateDestination,
        CancellationToken cancellationToken
    );
    Task DeleteDestinationAsync(long destinationId, CancellationToken cancellationToken);
}

using Zora.Core.Models;

namespace Zora.Core.Features.DestinationServices;

public interface IDestinationWriteService
{
    Task<Destination> CreateAsync(
        CreateDestination createDestination,
        CancellationToken cancellationToken
    );
    Task<Destination?> UpdateAsync(
        long destinationId,
        UpdateDestination updateDestination,
        CancellationToken cancellationToken
    );
    Task DeleteAsync(long destinationId, CancellationToken cancellationToken);
}

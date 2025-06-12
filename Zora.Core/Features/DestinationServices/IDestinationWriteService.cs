using System.Threading;
using System.Threading.Tasks;
using Zora.Core.Features.DestinationServices.Models;

namespace Zora.Core.Features.DestinationServices;

public interface IDestinationWriteService
{
    Task<Destination> CreateDestinationAsync(
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

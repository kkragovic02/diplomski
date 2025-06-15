using System.Threading;
using System.Threading.Tasks;
using Zora.Core.Features.DestinationServices.Models;

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

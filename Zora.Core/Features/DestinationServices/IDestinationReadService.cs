using Zora.Core.Features.DestinationServices.Models;

namespace Zora.Core.Features.DestinationServices;

public interface IDestinationReadService
{
    Task<Destination?> GetDestinationByIdAsync(long id, CancellationToken cancellationToken);
}

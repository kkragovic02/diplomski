namespace Zora.Core.DestinationServices;

using Zora.Core.DestinationServices.Models;

public interface IDestinationReadService
{
    Task<Destination?> GetDestinationByIdAsync(long id, CancellationToken cancellationToken);
}

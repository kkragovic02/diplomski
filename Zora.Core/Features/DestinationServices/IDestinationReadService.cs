using Zora.Core.Models;

namespace Zora.Core.Features.DestinationServices;

public interface IDestinationReadService
{
    Task<Destination?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<List<Destination>> GetAllAsync(CancellationToken cancellationToken);
}

using System.Threading;
using System.Threading.Tasks;
using Zora.Core.Features.DestinationServices.Models;

namespace Zora.Core.Features.DestinationServices;

public interface IDestinationReadService
{
    Task<Destination?> GetByIdAsync(long id, CancellationToken cancellationToken);
}

using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Features.DestinationServices.Models;

namespace Zora.Core.Features.DestinationServices;

internal class DestinationReadService(ZoraDbContext dbContext) : IDestinationReadService
{
    public async Task<Destination?> GetDestinationByIdAsync(
        long destinationId,
        CancellationToken cancellationToken
    )
    {
        var destination = await dbContext
            .Destinations.AsNoTracking()
            .FirstOrDefaultAsync(destination => destination.Id == destinationId, cancellationToken);

        return destination == null
            ? null
            : new Destination(destination.Id, destination.Name, destination.Description);
    }
}

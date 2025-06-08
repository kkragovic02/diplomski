using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.DestinationServices.Models;

namespace Zora.Core.DestinationServices;

internal class DestinationReadService(ZoraDbContext dbContext) : IDestinationReadService
{
    public async Task<Destination?> GetDestinationByIdAsync(
        long destinationId,
        CancellationToken cancellationToken
    )
    {
        var destination = await dbContext.Destinations.FirstOrDefaultAsync(
            d => d.Id == destinationId,
            cancellationToken
        );

        return destination == null
            ? null
            : new Destination(destination.Id, destination.Name, destination.Description);
    }
}

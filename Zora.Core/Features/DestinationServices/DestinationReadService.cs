using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Models;

namespace Zora.Core.Features.DestinationServices;

internal class DestinationReadService(ZoraDbContext dbContext) : IDestinationReadService
{
    public async Task<Destination?> GetByIdAsync(
        long destinationId,
        CancellationToken cancellationToken
    )
    {
        var destinationModel = await dbContext
            .Destinations.AsNoTracking()
            .FirstOrDefaultAsync(destination => destination.Id == destinationId, cancellationToken);

        return destinationModel == null ? null : destinationModel.MapToDestination();
    }

    public async Task<List<Destination>> GetAllAsync(CancellationToken cancellationToken)
    {
        var models = await dbContext.Destinations.AsNoTracking().ToListAsync(cancellationToken);

        return models.Select(m => m.MapToDestination()).ToList();
    }
}

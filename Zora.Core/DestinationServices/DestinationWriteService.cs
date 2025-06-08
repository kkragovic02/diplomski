using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.DestinationServices.Models;

namespace Zora.Core.DestinationServices;

internal class DestinationWriteService(ZoraDbContext dbContext) : IDestinationWriteService
{
    public async Task<Destination?> CreateDestinationAsync(
        CreateDestination createDestination,
        CancellationToken cancellationToken
    )
    {
        var destinationModel = new DestinationModel
        {
            Name = createDestination.Name,
            Description = createDestination.Description,
        };

        dbContext.Destinations.Add(destinationModel);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new Destination(
            destinationModel.Id,
            destinationModel.Name,
            destinationModel.Description
        );
    }

    public async Task<Destination?> UpdateDestinationAsync(
        long destinationId,
        UpdateDestination updateDestination,
        CancellationToken cancellationToken
    )
    {
        var existing = await dbContext.Destinations.FirstOrDefaultAsync(
            d => d.Id == destinationId,
            cancellationToken
        );

        if (existing == null)
        {
            return null;
        }

        existing.Name = updateDestination.Name ?? existing.Name;
        existing.Description = updateDestination.Description ?? existing.Description;

        dbContext.Destinations.Update(existing);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new Destination(existing.Id, existing.Name, existing.Description);
        ;
    }

    public async Task DeleteDestinationAsync(
        long destinationId,
        CancellationToken cancellationToken
    )
    {
        await dbContext
            .Destinations.Where(d => d.Id == destinationId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}

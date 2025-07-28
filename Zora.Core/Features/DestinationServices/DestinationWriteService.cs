using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.Models;

namespace Zora.Core.Features.DestinationServices;

internal class DestinationWriteService(ZoraDbContext dbContext) : IDestinationWriteService
{
    public async Task<Destination> CreateAsync(
        CreateDestination createDestination,
        CancellationToken cancellationToken
    )
    {
        var existing = await dbContext.Destinations.AnyAsync(
            d => d.Name == createDestination.Name,
            cancellationToken
        );

        if (existing)
        {
            throw new InvalidOperationException(
                $"Destinacija sa imenom '{createDestination.Name}' već postoji."
            );
        }
        var destinationModel = new DestinationModel
        {
            Name = createDestination.Name,
            Description = createDestination.Description,
        };

        dbContext.Destinations.Add(destinationModel);
        await dbContext.SaveChangesAsync(cancellationToken);

        return destinationModel.MapToDestination();
    }

    public async Task<Destination?> UpdateAsync(
        long destinationId,
        UpdateDestination updateDestination,
        CancellationToken cancellationToken
    )
    {
        var destinationToUpdate = await dbContext.Destinations.FirstOrDefaultAsync(
            destination => destination.Id == destinationId,
            cancellationToken
        );

        if (destinationToUpdate == null)
        {
            return null;
        }

        destinationToUpdate.Name = updateDestination.Name ?? destinationToUpdate.Name;
        destinationToUpdate.Description =
            updateDestination.Description ?? destinationToUpdate.Description;

        dbContext.Destinations.Update(destinationToUpdate);
        await dbContext.SaveChangesAsync(cancellationToken);

        return destinationToUpdate.MapToDestination();
    }

    public async Task DeleteAsync(long destinationId, CancellationToken cancellationToken)
    {
        await dbContext
            .Destinations.Where(destination => destination.Id == destinationId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}

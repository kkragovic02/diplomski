using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.Features.DestinationServices.Models;

namespace Zora.Core.Features.DestinationServices;

internal class DestinationWriteService(ZoraDbContext dbContext) : IDestinationWriteService
{
    public async Task<Destination> CreateDestinationAsync(
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

        return MapToDestination(destinationModel);
    }

    public async Task<Destination?> UpdateDestinationAsync(
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

        return MapToDestination(destinationToUpdate);
    }

    public async Task DeleteDestinationAsync(
        long destinationId,
        CancellationToken cancellationToken
    )
    {
        await dbContext
            .Destinations.Where(destination => destination.Id == destinationId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    private static Destination MapToDestination(DestinationModel destinationModel)
    {
        return new Destination(
            destinationModel.Id,
            destinationModel.Name,
            destinationModel.Description
        );
    }
}

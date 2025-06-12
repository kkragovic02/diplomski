using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.Features.AttractionServices.Models;

namespace Zora.Core.Features.AttractionServices;

internal class AttractionWriteService(
    ZoraDbContext dbContext,
    ILogger<AttractionWriteService> logger
) : IAttractionWriteService
{
    public async Task<Attraction> CreateAttractionAsync(
        CreateAttraction createAttraction,
        CancellationToken cancellationToken
    )
    {
        var attractionModel = new AttractionModel { Name = createAttraction.Name };

        dbContext.Attractions.Add(attractionModel);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Attraction with ID {AttractionId} created successfully.",
            attractionModel.Id
        );

        return MapToAttraction(attractionModel);
    }

    public async Task<Attraction?> UpdateAttractionAsync(
        long attractionId,
        UpdateAttraction updateAttraction,
        CancellationToken cancellationToken
    )
    {
        var attractionToUpdate = await dbContext.Attractions.FirstOrDefaultAsync(
            attraction => attraction.Id == attractionId,
            cancellationToken
        );

        if (attractionToUpdate == null)
        {
            logger.LogWarning(
                "Attraction with ID {AttractionId} not found for update.",
                attractionId
            );

            return null;
        }

        attractionToUpdate.Name = updateAttraction.Name;
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Attraction with ID {AttractionId} updated successfully.",
            attractionId
        );

        return MapToAttraction(attractionToUpdate);
    }

    public async Task DeleteAttractionAsync(long id, CancellationToken cancellationToken)
    {
        await dbContext
            .Attractions.Where(attraction => attraction.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        logger.LogInformation("Attraction with ID {AttractionId} deleted successfully.", id);
    }

    private static Attraction MapToAttraction(AttractionModel attractionModel)
    {
        return new Attraction(attractionModel.Id, attractionModel.Name);
    }
}

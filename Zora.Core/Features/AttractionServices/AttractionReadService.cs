using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zora.Core.Database;
using Zora.Core.Models;

namespace Zora.Core.Features.AttractionServices;

internal class AttractionReadService(
    ZoraDbContext dbContext,
    ILogger<AttractionWriteService> logger
) : IAttractionReadService
{
    public async Task<IReadOnlyList<Attraction>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext
            .Attractions.AsNoTracking()
            .Select(attraction => new Attraction(attraction.Id, attraction.Name))
            .ToListAsync(cancellationToken);
    }

    public async Task<Attraction?> GetAttractionByIdAsync(
        long id,
        CancellationToken cancellationToken
    )
    {
        var attractionModel = await dbContext
            .Attractions.AsNoTracking()
            .FirstOrDefaultAsync(attraction => attraction.Id == id, cancellationToken);

        if (attractionModel == null)
        {
            logger.LogWarning("Attraction with ID {AttractionId} not found.", id);

            return null;
        }

        return attractionModel.MapToAttraction();
    }

    public async Task<IReadOnlyList<Attraction>> GetByTourIdAsync(
        long tourId,
        CancellationToken cancellationToken
    )
    {
        var attractionModels = await dbContext
            .Attractions.Include(attraction => attraction.Tours)
            .Where(attraction => attraction.Tours.Any(tour => tour.Id == tourId))
            .ToListAsync(cancellationToken);

        return attractionModels.ConvertAll(attraction => attraction.MapToAttraction());
    }
}

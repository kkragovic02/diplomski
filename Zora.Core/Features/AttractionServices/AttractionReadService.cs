using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.Features.AttractionServices.Models;

namespace Zora.Core.Features.AttractionServices;

internal class AttractionReadService(
    ZoraDbContext dbContext,
    ILogger<AttractionWriteService> logger
) : IAttractionReadService
{
    public async Task<IReadOnlyList<Attraction>> GetAllAttractionsAsync(
        CancellationToken cancellationToken
    )
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
        var attraction = await dbContext
            .Attractions.AsNoTracking()
            .FirstOrDefaultAsync(attraction => attraction.Id == id, cancellationToken);

        if (attraction == null)
        {
            logger.LogWarning("Attraction with ID {AttractionId} not found.", id);

            return null;
        }

        return new Attraction(attraction.Id, attraction.Name);
    }

    public async Task<IReadOnlyList<Attraction>> GetAttractionsByTourIdAsync(
        long tourId,
        CancellationToken cancellationToken
    )
    {
        var attractionModel = await dbContext
            .Attractions.Include(attraction => attraction.Tours)
            .Where(attraction => attraction.Tours.Any(tour => tour.Id == tourId))
            .ToListAsync(cancellationToken);

        return attractionModel.ConvertAll(MapToAttraction);
    }

    private static Attraction MapToAttraction(AttractionModel attractionModel)
    {
        return new Attraction(attractionModel.Id, attractionModel.Name);
    }
}

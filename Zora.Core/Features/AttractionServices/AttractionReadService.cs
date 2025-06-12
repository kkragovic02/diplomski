using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zora.Core.Database;
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
}

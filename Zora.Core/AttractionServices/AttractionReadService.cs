using Microsoft.EntityFrameworkCore;
using Zora.Core.AttractionServices.Models;
using Zora.Core.Database;

namespace Zora.Core.AttractionServices;

internal class AttractionReadService(ZoraDbContext dbContext) : IAttractionReadService
{
    public async Task<List<Attraction>> GetAllAttractionsAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await dbContext
            .Attractions.AsNoTracking()
            .Select(a => new Attraction(a.Id, a.Name))
            .ToListAsync(cancellationToken);
    }

    public async Task<Attraction?> GetAttractionByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var attraction = await dbContext
            .Attractions.AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        return attraction == null ? null : new Attraction(attraction.Id, attraction.Name);
    }
}

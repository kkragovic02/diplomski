using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Database.Models;

namespace Zora.Core.Features.GalleryServices;

internal class GalleryReadService(ZoraDbContext dbContext) : IGalleryReadService
{
    public async Task<List<GalleryModel>> GetAllForTourAsync(
        long tourId,
        CancellationToken cancellationToken
    )
    {
        return await dbContext
            .Galleries.AsNoTracking()
            .Where(g => g.TourId == tourId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<GalleryModel>> GetAllForDestinationAsync(
        long destinationId,
        CancellationToken cancellationToken
    )
    {
        return await dbContext
            .Galleries.AsNoTracking()
            .Where(g => g.Tour.DestinationId == destinationId)
            .ToListAsync(cancellationToken);
    }
}

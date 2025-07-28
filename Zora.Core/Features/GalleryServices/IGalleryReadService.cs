namespace Zora.Core.Features.GalleryServices;

using Zora.Core.Database.Models;

public interface IGalleryReadService
{
    Task<List<GalleryModel>> GetAllForTourAsync(long tourId, CancellationToken cancellationToken);
    Task<List<GalleryModel>> GetAllForDestinationAsync(
        long destinationId,
        CancellationToken cancellationToken
    );
}

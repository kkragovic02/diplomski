using Microsoft.AspNetCore.Http;
using Zora.Core.Database.Models;

namespace Zora.Core.Features.GalleryServices;

public interface IGalleryWriteService
{
    Task<GalleryModel> UploadAsync(
        long tourId,
        IFormFile image,
        CancellationToken cancellationToken
    );
    Task<bool> DeleteAsync(long galleryId, CancellationToken cancellationToken);
}

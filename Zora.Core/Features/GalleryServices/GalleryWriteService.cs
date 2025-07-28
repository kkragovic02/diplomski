using Microsoft.AspNetCore.Http;
using Zora.Core.Database;
using Zora.Core.Database.Models;

namespace Zora.Core.Features.GalleryServices;

internal class GalleryWriteService(ZoraDbContext dbContext) : IGalleryWriteService
{
    public async Task<GalleryModel> UploadAsync(
        long tourId,
        IFormFile image,
        CancellationToken cancellationToken
    )
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
        var relativePath = Path.Combine("wwwroot", "images", "gallery", fileName);
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await image.CopyToAsync(stream, cancellationToken);

        var galleryModel = new GalleryModel
        {
            TourId = tourId,
            FileName = fileName,
            FilePath = Path.Combine("images", "gallery", fileName).Replace("\\", "/"),
        };

        dbContext.Galleries.Add(galleryModel);
        await dbContext.SaveChangesAsync(cancellationToken);

        return galleryModel;
    }

    public async Task<bool> DeleteAsync(long galleryId, CancellationToken cancellationToken)
    {
        var image = await dbContext.Galleries.FindAsync([galleryId], cancellationToken);
        if (image is null)
            return false;

        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.FilePath);
        if (File.Exists(fullPath))
            File.Delete(fullPath);

        dbContext.Galleries.Remove(image);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}

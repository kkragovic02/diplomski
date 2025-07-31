using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.Models;

namespace Zora.Core.Features.StoryServices;

internal class StoryWriteService(ZoraDbContext dbContext) : IStoryWriteService
{
    public async Task<Story> CreateAsync(
        CreateStory story,
        List<IFormFile> images,
        CancellationToken cancellationToken
    )
    {
        var storyModel = new StoryModel
        {
            UserId = story.UserId,
            TourId = story.TourId,
            Content = story.Content,
            CreatedAt = story.CreatedAt,
        };

        dbContext.Stories.Add(storyModel);
        await dbContext.SaveChangesAsync(cancellationToken);

        var uploadedImages = new List<StoryImageModel>();
        foreach (var image in images)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var relativePath = Path.Combine("wwwroot", "images", "stories", fileName);
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await image.CopyToAsync(stream, cancellationToken);

            uploadedImages.Add(
                new StoryImageModel
                {
                    StoryId = storyModel.Id,
                    FileName = fileName,
                    FilePath = Path.Combine("images", "stories", fileName).Replace("\\", "/"),
                }
            );
        }

        dbContext.StoryImages.AddRange(uploadedImages);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new Story
        {
            Id = storyModel.Id,
            UserId = storyModel.UserId,
            TourId = storyModel.TourId,
            Content = storyModel.Content,
            ImagePaths = uploadedImages.Select(i => i.FilePath).ToList(),
        };
    }

    public async Task<bool> DeleteAsync(long storyId, CancellationToken cancellationToken)
    {
        var story = await dbContext
            .Stories.Include(s => s.Images)
            .FirstOrDefaultAsync(s => s.Id == storyId, cancellationToken);
        if (story is null)
            return false;

        foreach (var image in story.Images)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.FilePath);
            if (File.Exists(path))
                File.Delete(path);
        }

        dbContext.Stories.Remove(story);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<Story?> UpdateAsync(
        long storyId,
        CreateStory story,
        List<string> keepImagePaths,
        List<IFormFile> newImages,
        CancellationToken cancellationToken
    )
    {
        var existing = await dbContext
            .Stories.Include(s => s.Images)
            .FirstOrDefaultAsync(s => s.Id == storyId, cancellationToken);

        if (existing is null)
            return null;

        var imagesToDelete = existing
            .Images.Where(img => !keepImagePaths.Contains(img.FilePath))
            .ToList();

        foreach (var image in imagesToDelete)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.FilePath);
            if (File.Exists(path))
                File.Delete(path);
        }
        dbContext.StoryImages.RemoveRange(imagesToDelete);

        var uploadedImages = new List<StoryImageModel>();
        foreach (var image in newImages)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var relativePath = Path.Combine("images", "stories", fileName).Replace("\\", "/");
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await image.CopyToAsync(stream, cancellationToken);

            uploadedImages.Add(
                new StoryImageModel
                {
                    StoryId = existing.Id,
                    FileName = fileName,
                    FilePath = relativePath,
                }
            );
        }
        dbContext.StoryImages.AddRange(uploadedImages);

        existing.Content = story.Content;
        existing.TourId = story.TourId;
        await dbContext.SaveChangesAsync(cancellationToken);

        var allImagePaths = existing
            .Images.Where(img => keepImagePaths.Contains(img.FilePath))
            .Select(img => img.FilePath)
            .Concat(uploadedImages.Select(i => i.FilePath))
            .ToList();

        return new Story
        {
            Id = existing.Id,
            UserId = existing.UserId,
            TourId = existing.TourId,
            Content = existing.Content,
            CreatedAt = existing.CreatedAt,
            ImagePaths = allImagePaths,
        };
    }
}

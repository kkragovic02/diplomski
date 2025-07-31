using Microsoft.AspNetCore.Http;
using Zora.Core.Models;

namespace Zora.Core.Features.StoryServices;

public interface IStoryWriteService
{
    Task<Story> CreateAsync(
        CreateStory story,
        List<IFormFile> images,
        CancellationToken cancellationToken
    );
    Task<bool> DeleteAsync(long storyId, CancellationToken cancellationToken);

    Task<Story?> UpdateAsync(
        long storyId,
        CreateStory story,
        List<string> keepImagePaths,
        List<IFormFile> newImages,
        CancellationToken cancellationToken
    );
}

using Zora.Core.Models;

namespace Zora.Core.Features.StoryServices;

public interface IStoryReadService
{
    Task<List<Story>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<Story>> GetByTourIdAsync(long tourId, CancellationToken cancellationToken);
    Task<List<Story>> GetByUserIdAsync(long userId, CancellationToken cancellationToken);
}

using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Models;

namespace Zora.Core.Features.StoryServices;

internal class StoryReadService(ZoraDbContext dbContext) : IStoryReadService
{
    public async Task<List<Story>> GetAllAsync(CancellationToken cancellationToken)
    {
        var stories = await dbContext
            .Stories.Include(s => s.Images)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return stories
            .Select(s => new Story
            {
                Id = s.Id,
                UserId = s.UserId,
                TourId = s.TourId,
                Content = s.Content,
                ImagePaths = s.Images.Select(i => i.FilePath).ToList(),
            })
            .ToList();
    }

    public async Task<List<Story>> GetByTourIdAsync(
        long tourId,
        CancellationToken cancellationToken
    )
    {
        return await dbContext
            .Stories.Where(s => s.TourId == tourId)
            .Include(s => s.Images)
            .AsNoTracking()
            .Select(s => new Story
            {
                Id = s.Id,
                UserId = s.UserId,
                TourId = s.TourId,
                Content = s.Content,
                CreatedAt = s.CreatedAt,
                ImagePaths = s.Images.Select(i => i.FilePath).ToList(),
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Story>> GetByUserIdAsync(
        long userId,
        CancellationToken cancellationToken
    )
    {
        return await dbContext
            .Stories.Where(s => s.UserId == userId)
            .Include(s => s.Images)
            .AsNoTracking()
            .Select(s => new Story
            {
                Id = s.Id,
                UserId = s.UserId,
                TourId = s.TourId,
                Content = s.Content,
                CreatedAt = s.CreatedAt,
                ImagePaths = s.Images.Select(i => i.FilePath).ToList(),
            })
            .ToListAsync(cancellationToken);
    }
}

using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Models;

namespace Zora.Core.Features.UserServices;

internal class UserReadService(ZoraDbContext dbContext) : IUserReadService
{
    public async Task<List<User>> GetAllAsync(
        CancellationToken cancellationToken,
        string? userName = null
    )
    {
        var userModels = await dbContext
            .Users.Select(user => user.MapToUser())
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);

        var filteredUsers = string.IsNullOrEmpty(userName)
            ? userModels
            : userModels
                .Where(u => u.Name.Contains(userName, StringComparison.OrdinalIgnoreCase))
                .ToList();

        return filteredUsers;
    }

    public async Task<bool> IsUserJoinedTourAsync(
        long userId,
        long tourId,
        CancellationToken cancellationToken
    )
    {
        return await dbContext
            .Tours.AsNoTracking()
            .Where(t => t.Id == tourId)
            .SelectMany(t => t.Participants)
            .AnyAsync(u => u.Id == userId, cancellationToken);
    }
}

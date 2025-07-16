using Zora.Core.Models;

namespace Zora.Core.Features.UserServices;

public interface IUserReadService
{
    Task<List<User>> GetAllAsync(CancellationToken cancellationToken, string? userName = null);
    Task<bool> IsUserJoinedTourAsync(long userId, long tourId, CancellationToken cancellationToken);
}

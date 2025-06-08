using Zora.Core.UserServices.Models;

namespace Zora.Core.UserServices;

public interface IUserWriteService
{
    public Task<User> CreateUserAsync(CreateUser user, CancellationToken cancellationToken);

    public Task<User?> UpdateUserAsync(
        long userId,
        UpdateUser user,
        CancellationToken cancellationToken
    );

    public Task DeleteUserAsync(long userId, CancellationToken cancellationToken);

    public Task<bool> JoinTourAsync(long userId, long tourId, CancellationToken cancellationToken);

    public Task<bool> LeaveTourAsync(long userId, long tourId, CancellationToken cancellationToken);

    public Task UpdateCheckListItemAsync(
        long userId,
        long tourId,
        long checkListItemId,
        bool isChecked,
        CancellationToken cancellationToken
    );
}

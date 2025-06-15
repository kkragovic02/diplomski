using System.Threading;
using System.Threading.Tasks;
using Zora.Core.Features.UserServices.Models;

namespace Zora.Core.Features.UserServices;

public interface IUserWriteService
{
    public Task<User> CreateAsync(CreateUser user, CancellationToken cancellationToken);

    public Task<User?> UpdateAsync(
        long userId,
        UpdateUser user,
        CancellationToken cancellationToken
    );

    public Task DeleteAsync(long userId, CancellationToken cancellationToken);

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

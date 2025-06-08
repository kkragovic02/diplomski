using Zora.Core.UserServices.Models;

namespace Zora.Core.UserServices;

public interface IUserReadService
{
    Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken, string? name = null);
}

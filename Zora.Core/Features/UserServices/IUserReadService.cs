using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zora.Core.Features.UserServices.Models;

namespace Zora.Core.Features.UserServices;

public interface IUserReadService
{
    Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken, string? UserName = null);
}

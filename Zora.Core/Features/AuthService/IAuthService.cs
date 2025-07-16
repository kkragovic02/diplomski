using Zora.Core.Database.Models;
using Zora.Core.Models;

namespace Zora.Core.Features.AuthService;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<User> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
}

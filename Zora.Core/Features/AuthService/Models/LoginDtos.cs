using Zora.Core.Models;

namespace Zora.Core.Database.Models;

public record LoginRequest(string Email, string Password, bool RememberMe);

public record LoginResponse(string Token, User User);

public record RegisterRequest(string Name, string Email, string Password);

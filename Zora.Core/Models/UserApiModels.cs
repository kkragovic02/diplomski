using Zora.Core.Database.Models;

namespace Zora.Core.Models;

public record User(long Id, string Name, string Email, Role Role);

public record CreateUser(string Name, string Email, string Password);

public record UpdateUser(string? Name, string? Email, string? Password, Role? Role);

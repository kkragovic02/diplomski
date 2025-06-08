using Zora.Core.Database.Models;

namespace Zora.Core.UserServices.Models;

public record User(long Id, string Name, string Email, Role Role);

public record CreateUser(string Name, string Email, string Password);

public class UpdateUser
{
    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }
    public Role? Role { get; set; }
};

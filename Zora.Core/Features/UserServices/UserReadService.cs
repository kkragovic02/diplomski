using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.Features.UserServices.Models;

namespace Zora.Core.Features.UserServices;

internal class UserReadService(ZoraDbContext dbContext) : IUserReadService
{
    public async Task<List<User>> GetAllUsersAsync(
        CancellationToken cancellationToken,
        string? name = null
    )
    {
        var users = await dbContext
            .Users.Select(u => MapToUseDto(u))
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);

        var filteredUsers = string.IsNullOrEmpty(name)
            ? users
            : users.Where(u => u.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

        return filteredUsers;
    }

    private static User MapToUseDto(UserModel userModel)
    {
        return new User(userModel.Id, userModel.Name, userModel.Email, userModel.Role);
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.UserServices.Models;

namespace Zora.Core.UserServices;

internal class UserWriteService(PasswordHasher<UserModel> passwordHasher, ZoraDbContext dbContext)
    : IUserWriteService
{
    public async Task<User> CreateUserAsync(
        CreateUser createUser,
        CancellationToken cancellationToken
    )
    {
        var user = MapToUserModel(createUser);
        user.PasswordHash = passwordHasher.HashPassword(user, createUser.Password);

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new User(user.Id, user.Name, user.Email, user.Role);
    }

    public async Task<User?> UpdateUserAsync(
        long userId,
        UpdateUser updateUser,
        CancellationToken cancellationToken
    )
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(
            user => user.Id == userId,
            cancellationToken
        );

        if (user == null)
        {
            return null;
        }

        user.Name = updateUser.Name ?? user.Name;
        user.Email = updateUser.Email ?? user.Email;
        user.Role = updateUser.Role ?? user.Role;
        if (updateUser.Password != null)
        {
            user.PasswordHash = passwordHasher.HashPassword(user, updateUser.Password);
        }

        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new User(user.Id, user.Name, user.Email, user.Role);
    }

    public Task DeleteUserAsync(long userId, CancellationToken cancellationToken)
    {
        return dbContext
            .Users.Where(user => user.Id == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    private static UserModel MapToUserModel(CreateUser createUser)
    {
        return new UserModel
        {
            Name = createUser.Name,
            Email = createUser.Email,
            Role = Role.Member,
        };
    }

    public async Task<bool> JoinTourAsync(
        long userId,
        long tourId,
        CancellationToken cancellationToken
    )
    {
        var user = await dbContext
            .Users.Include(u => u.UserTours)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        var tour = await dbContext
            .Tours.Include(t => t.Participants)
            .FirstOrDefaultAsync(t => t.Id == tourId, cancellationToken);

        if (user == null || tour == null || tour.Participants.Contains(user))
            return false;

        tour.Participants.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> LeaveTourAsync(
        long userId,
        long tourId,
        CancellationToken cancellationToken
    )
    {
        var user = await dbContext
            .Users.Include(u => u.UserTours)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        var tour = await dbContext
            .Tours.Include(t => t.Participants)
            .FirstOrDefaultAsync(t => t.Id == tourId, cancellationToken);

        if (user == null || tour == null || !tour.Participants.Contains(user))
            return false;

        tour.Participants.Remove(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task UpdateCheckListItemAsync(
        long userId,
        long tourId,
        long equipmentId,
        bool isChecked,
        CancellationToken cancellationToken
    )
    {
        var item = await dbContext.UserCheckLists.FirstOrDefaultAsync(
            i => i.UserId == userId && i.TourId == tourId && i.EquipmentId == equipmentId,
            cancellationToken
        );

        if (item == null)
        {
            dbContext.UserCheckLists.Add(
                new CheckListItem
                {
                    UserId = userId,
                    TourId = tourId,
                    EquipmentId = equipmentId,
                    IsChecked = isChecked,
                }
            );
        }
        else
        {
            item.IsChecked = isChecked;
            dbContext.UserCheckLists.Update(item);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

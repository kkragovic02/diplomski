using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.Models;

namespace Zora.Core.Features.UserServices;

internal class UserWriteService(
    PasswordHasher<UserModel> passwordHasher,
    ZoraDbContext dbContext,
    ILogger<UserWriteService> logger
) : IUserWriteService
{
    private const string JoinTour = "join";
    private const string LeaveTour = "leave";

    public async Task<User> CreateAsync(CreateUser createUser, CancellationToken cancellationToken)
    {
        var userModel = new UserModel
        {
            Name = createUser.Name,
            Email = createUser.Email,
            Role = Role.Member,
        };

        userModel.PasswordHash = passwordHasher.HashPassword(userModel, createUser.Password);

        dbContext.Users.Add(userModel);
        await dbContext.SaveChangesAsync(cancellationToken);

        return userModel.MapToUser();
    }

    public async Task<User?> UpdateAsync(
        long userId,
        UpdateUser updateUser,
        CancellationToken cancellationToken
    )
    {
        var userModel = await dbContext.Users.FirstOrDefaultAsync(
            user => user.Id == userId,
            cancellationToken
        );

        if (userModel == null)
        {
            return null;
        }

        userModel.Name = updateUser.Name ?? userModel.Name;
        userModel.Email = updateUser.Email ?? userModel.Email;
        userModel.Role = updateUser.Role ?? userModel.Role;
        if (updateUser.Password != null)
        {
            userModel.PasswordHash = passwordHasher.HashPassword(userModel, updateUser.Password);
        }

        dbContext.Users.Update(userModel);
        await dbContext.SaveChangesAsync(cancellationToken);

        return userModel.MapToUser();
    }

    public Task DeleteAsync(long userId, CancellationToken cancellationToken)
    {
        return dbContext
            .Users.Where(user => user.Id == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<bool> JoinTourAsync(
        long userId,
        long tourId,
        CancellationToken cancellationToken
    )
    {
        var (userModel, tourModel) = await GetUserAndTourModels(userId, tourId, cancellationToken);

        if (!IsValid(userId, tourId, userModel, tourModel, JoinTour))
        {
            return false;
        }
        if (tourModel!.AvailableSpots <= 0)
        {
            logger.LogError("Tour {TourId} is full. Cannot join.", tourId);
            return false;
        }
        tourModel.AvailableSpots--;
        tourModel!.Participants.Add(userModel!);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> LeaveTourAsync(
        long userId,
        long tourId,
        CancellationToken cancellationToken
    )
    {
        var (userModel, tourModel) = await GetUserAndTourModels(userId, tourId, cancellationToken);

        if (!IsValid(userId, tourId, userModel, tourModel, LeaveTour))
        {
            return false;
        }
        tourModel.AvailableSpots++;
        tourModel!.Participants.Remove(userModel!);
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
        var checkListItemModel = await dbContext.UserCheckLists.FirstOrDefaultAsync(
            i => i.UserId == userId && i.TourId == tourId && i.EquipmentId == equipmentId,
            cancellationToken
        );

        if (checkListItemModel == null)
        {
            dbContext.UserCheckLists.Add(
                new CheckListItemModel
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
            checkListItemModel.IsChecked = isChecked;
            dbContext.UserCheckLists.Update(checkListItemModel);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<(UserModel? userModel, TourModel? tourModel)> GetUserAndTourModels(
        long userId,
        long tourId,
        CancellationToken cancellationToken
    )
    {
        var userModel = await dbContext
            .Users.Include(u => u.UserTours)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        var tourModel = await dbContext
            .Tours.Include(t => t.Participants)
            .FirstOrDefaultAsync(t => t.Id == tourId, cancellationToken);

        return (userModel, tourModel);
    }

    private bool IsValid(
        long userId,
        long tourId,
        UserModel? userModel,
        TourModel? tourModel,
        string choice
    )
    {
        if (userModel == null)
        {
            logger.LogError("User {UserId} not found", userId);
            return false;
        }

        if (tourModel == null)
        {
            logger.LogError("Tour with ID {TourId} does not exist", tourId);
            return false;
        }

        var userInTour = tourModel.Participants.Contains(userModel);

        if (choice == JoinTour)
        {
            if (userInTour)
            {
                logger.LogError(
                    "User {UserId} is already signed up for Tour {TourId}",
                    userId,
                    tourId
                );
                return false;
            }
        }
        else if (choice == LeaveTour)
        {
            if (!userInTour)
            {
                logger.LogWarning(
                    "User {UserId} is already not signed up for Tour {TourId}",
                    userId,
                    tourId
                );
                return false;
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(choice));
        }

        return true;
    }
}

using Zora.Core.Database.Models;

namespace Zora.Core.Models;

public static class ZoraModelMapper
{
    public static Attraction MapToAttraction(this AttractionModel attractionModel)
    {
        return new Attraction(attractionModel.Id, attractionModel.Name);
    }

    public static Destination MapToDestination(this DestinationModel destinationModel)
    {
        return new Destination(
            destinationModel.Id,
            destinationModel.Name,
            destinationModel.Description
        );
    }

    public static DiaryNote MapToDiaryNote(this DiaryNoteModel diaryNoteModel)
    {
        return new DiaryNote(
            diaryNoteModel.Id,
            diaryNoteModel.Title,
            diaryNoteModel.Content,
            diaryNoteModel.CreatedAt.DateTime,
            diaryNoteModel.UserId,
            diaryNoteModel.TourId
        );
    }

    public static Equipment MapToEquipment(this EquipmentModel equipmentModel)
    {
        return new Equipment(equipmentModel.Id, equipmentModel.Name);
    }

    public static Tour MapToTour(this TourModel tourModel)
    {
        return new Tour(
            tourModel.Id,
            tourModel.Name,
            tourModel.Description,
            tourModel.Distance,
            tourModel.Duration,
            tourModel.ElevationGain,
            tourModel.AvailableSpots,
            tourModel.ScheduledAt.DateTime,
            tourModel.DestinationId,
            tourModel.Destination?.Name ?? "Nepoznato",
            tourModel.GuideId,
            tourModel.Equipment.Select(e => e.Id).ToList(),
            tourModel.Attractions.Select(a => a.Id).ToList()
        );
    }

    public static TourForCalendar MapToCalendarDto(this TourModel tour)
    {
        return new TourForCalendar(
            tour.Id,
            tour.Name,
            tour.Distance,
            tour.ElevationGain,
            tour.AvailableSpots,
            tour.ScheduledAt.DateTime,
            tour.GuideId,
            tour.Destination?.Name ?? "Nepoznato"
        );
    }

    public static User MapToUser(this UserModel userModel)
    {
        return new User(userModel.Id, userModel.Name, userModel.Email, userModel.Role);
    }

    public static CheckListItem MapToCheckListItem(this CheckListItemModel checkListItemModel)
    {
        return new CheckListItem(
            checkListItemModel.Id,
            checkListItemModel.IsChecked,
            checkListItemModel.UserId,
            checkListItemModel.TourId,
            checkListItemModel.EquipmentId
        );
    }
}

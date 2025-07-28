namespace Zora.Core.Models;

public record CheckListItem(long Id, bool IsChecked, long UserId, long TourId, long EquipmentId);

public record CreateCheckListItem(bool IsChecked, long UserId, long TourId, long EquipmentId);

public record UserCheckListItemDto(long EquipmentId, string Name, bool IsChecked);

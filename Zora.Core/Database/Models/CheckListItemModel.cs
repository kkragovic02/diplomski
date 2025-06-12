namespace Zora.Core.Database.Models;

public class CheckListItemModel
{
    public long Id { get; set; }

    public required bool IsChecked { get; set; }

    public long UserId { get; set; }

    public UserModel? User { get; set; }

    public long TourId { get; set; }

    public TourModel? Tour { get; set; }

    public long EquipmentId { get; set; }

    public EquipmentModel? Equipment { get; set; }
}

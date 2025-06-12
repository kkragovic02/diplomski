using System.ComponentModel.DataAnnotations;

namespace Zora.Core.Database.Models;

public class UserModel
{
    public long Id { get; set; }

    [MaxLength(50)]
    public required string Name { get; set; }

    [MaxLength(50)]
    public required string Email { get; set; }

    [MaxLength(256)]
    public string? PasswordHash { get; set; }

    public required Role Role { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public ICollection<DiaryNoteModel> DiaryNotes { get; set; } = new List<DiaryNoteModel>();

    public ICollection<TourModel> UserTours { get; set; } = new List<TourModel>();

    public ICollection<TourModel> GuideTours { get; set; } = new List<TourModel>();

    public ICollection<CheckListItemModel> UserCheckList { get; set; } =
        new List<CheckListItemModel>();
}

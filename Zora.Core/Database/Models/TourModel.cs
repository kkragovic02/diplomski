using System.ComponentModel.DataAnnotations;

namespace Zora.Core.Database.Models;

public class TourModel
{
    public long Id { get; set; }

    [MaxLength(50)]
    public required string Name { get; set; }

    [MaxLength(500)]
    public required string Description { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public double Distance { get; set; }

    public TimeSpan Duration { get; set; }

    public double ElevationGain { get; set; }

    public int AvailableSpots { get; set; }

    public DateTimeOffset ScheduledAt { get; set; }

    public long GuideId { get; set; }

    public UserModel? Guide { get; set; }

    public long DestinationId { get; set; }

    public DestinationModel? Destination { get; set; }

    public ICollection<DiaryNoteModel>? DiaryNotes { get; set; } = new List<DiaryNoteModel>();

    public ICollection<EquipmentModel> Equipment { get; set; } = new List<EquipmentModel>();

    public ICollection<AttractionModel> Attractions { get; set; } = new List<AttractionModel>();

    public ICollection<UserModel> Participants { get; set; } = new List<UserModel>();
}

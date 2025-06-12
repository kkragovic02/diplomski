namespace Zora.Core.TourServices.Models;

public class CreateTour
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public double Distance { get; set; }

    public TimeSpan Duration { get; set; }

    public double ElevationGain { get; set; }

    public int AvailableSpots { get; set; }

    public DateTime ScheduledAt { get; set; }

    public long DestinationId { get; set; }

    public long GuideId { get; set; }

    public List<int> AttractionIds { get; set; } = [];

    public List<int> EquipmentIds { get; set; } = [];
}

public class UpdateTour
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double? Distance { get; set; }
    public TimeSpan? Duration { get; set; }
    public double? ElevationGain { get; set; }
    public int? AvailableSpots { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public long? DestinationId { get; set; }

    public List<long>? AttractionIds { get; set; }

    public List<long>? EquipmentIds { get; set; }
}

public class Tour
{
    public long Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public double Distance { get; set; }
    public TimeSpan Duration { get; set; }
    public double ElevationGain { get; set; }
    public int AvailableSpots { get; set; }
    public DateTime ScheduledAt { get; set; }
    public long DestinationId { get; set; }
    public long GuideId { get; set; }

    public List<int> AttractionIds { get; set; } = [];

    public List<int> EquipmentIds { get; set; } = [];
}

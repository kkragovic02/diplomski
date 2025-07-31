using System.ComponentModel.DataAnnotations;

namespace Zora.Core.Models;

public record CreateTour(
    string Name,
    string? Description,
    double Distance,
    TimeSpan Duration,
    double ElevationGain,
    int AvailableSpots,
    DateTime ScheduledAt,
    long DestinationId,
    long GuideId,
    List<long> AttractionIds,
    List<long> EquipmentIds
);

public record UpdateTour(
    string? Name,
    string? Description,
    double? Distance,
    TimeSpan? Duration,
    double? ElevationGain,
    int? AvailableSpots,
    DateTime? ScheduledAt,
    long? DestinationId,
    List<long>? AttractionIds,
    List<long>? EquipmentIds
);

public record Tour(
    long Id,
    string Name,
    string Description,
    double Distance,
    TimeSpan Duration,
    double ElevationGain,
    int AvailableSpots,
    DateTime ScheduledAt,
    long DestinationId,
    string DestinationName,
    long GuideId,
    List<long> AttractionIds,
    List<long> EquipmentIds
);

public record TourWithGuideInfo(
    long Id,
    string Name,
    string Destination,
    double Distance,
    double ElevationGain,
    int AvailableSpots,
    DateTime ScheduledAt,
    string GuideEmail,
    string Description
);

public record TourForCalendar(
    long Id,
    string Name,
    double Distance,
    double ElevationGain,
    int AvailableSpots,
    DateTime ScheduledAt,
    long GuideId,
    string DestinationName
);

public class TourSearchParameters
{
    public DateTimeOffset? DateFrom { get; set; }
    public DateTimeOffset? DateTo { get; set; }
    public List<long>? AttractionIds { get; set; }
    public double? DistanceFrom { get; set; }
    public double? DistanceTo { get; set; }
    public double? ElevationFrom { get; set; }
    public double? ElevationTo { get; set; }
}

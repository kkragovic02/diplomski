using System;
using System.Collections.Generic;

namespace Zora.Core.Features.TourServices.Models;

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
    long GuideId,
    List<long> AttractionIds,
    List<long> EquipmentIds
);

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.Features.TourServices.Models;

namespace Zora.Core.Features.TourServices;

internal class TourWriteService(ZoraDbContext dbContext) : ITourWriteService
{
    public async Task<Tour> CreateTourAsync(
        CreateTour createTour,
        CancellationToken cancellationToken
    )
    {
        var tourModel = new TourModel
        {
            Name = createTour.Name,
            Description = createTour.Description ?? string.Empty,
            Distance = createTour.Distance,
            Duration = createTour.Duration,
            ElevationGain = createTour.ElevationGain,
            AvailableSpots = createTour.AvailableSpots,
            ScheduledAt = createTour.ScheduledAt,
            DestinationId = createTour.DestinationId,
            GuideId = createTour.GuideId,
        };

        if (createTour.EquipmentIds?.Count > 0)
        {
            tourModel.Equipment = await dbContext
                .Equipments.Where(equipment => createTour.EquipmentIds.Contains((int)equipment.Id))
                .ToListAsync(cancellationToken);
        }

        if (createTour.AttractionIds?.Count > 0)
        {
            tourModel.Attractions = await dbContext
                .Attractions.Where(attraction =>
                    createTour.AttractionIds.Contains((int)attraction.Id)
                )
                .ToListAsync(cancellationToken);
        }

        dbContext.Tours.Add(tourModel);
        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToTour(tourModel);
    }

    public async Task<Tour?> UpdateTourAsync(
        long tourId,
        UpdateTour updateTour,
        CancellationToken cancellationToken
    )
    {
        var tourModel = await dbContext.Tours.FirstOrDefaultAsync(
            tour => tour.Id == tourId,
            cancellationToken
        );

        if (tourModel == null)
            return null;

        tourModel.Name = updateTour.Name ?? tourModel.Name;
        tourModel.Description = updateTour.Description ?? tourModel.Description;
        tourModel.Distance = updateTour.Distance ?? tourModel.Distance;
        tourModel.Duration = updateTour.Duration ?? tourModel.Duration;
        tourModel.ElevationGain = updateTour.ElevationGain ?? tourModel.ElevationGain;
        tourModel.AvailableSpots = updateTour.AvailableSpots ?? tourModel.AvailableSpots;
        tourModel.ScheduledAt = updateTour.ScheduledAt ?? tourModel.ScheduledAt;
        tourModel.DestinationId = updateTour.DestinationId ?? tourModel.DestinationId;

        if (updateTour.EquipmentIds?.Count > 0)
        {
            tourModel.Equipment = await dbContext
                .Equipments.Where(equipment => updateTour.EquipmentIds.Contains(equipment.Id))
                .ToListAsync(cancellationToken);
        }

        if (updateTour.AttractionIds?.Count > 0)
        {
            tourModel.Attractions = await dbContext
                .Attractions.Where(attraction => updateTour.AttractionIds.Contains(attraction.Id))
                .ToListAsync(cancellationToken);
        }

        tourModel.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToTour(tourModel);
    }

    public async Task<bool> DeleteTourAsync(long tourId, CancellationToken cancellationToken)
    {
        var tour = await dbContext.Tours.FirstOrDefaultAsync(
            tour => tour.Id == tourId,
            cancellationToken
        );

        if (tour == null)
        {
            return false;
        }

        dbContext.Tours.Remove(tour);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static Tour MapToTour(TourModel tourModel) =>
        new Tour(
            tourModel.Id,
            tourModel.Name,
            tourModel.Description,
            tourModel.Distance,
            tourModel.Duration,
            tourModel.ElevationGain,
            tourModel.AvailableSpots,
            tourModel.ScheduledAt.DateTime,
            tourModel.DestinationId,
            tourModel.GuideId,
            tourModel.Equipment.Select(e => e.Id).ToList(),
            tourModel.Attractions.Select(a => a.Id).ToList()
        );
}

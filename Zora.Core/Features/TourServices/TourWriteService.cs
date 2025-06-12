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
        var model = new TourModel
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
            model.Equipment = await dbContext
                .Equipments.Where(e => createTour.EquipmentIds.Contains((int)e.Id))
                .ToListAsync(cancellationToken);
        }

        if (createTour.AttractionIds?.Count > 0)
        {
            model.Attractions = await dbContext
                .Attractions.Where(a => createTour.AttractionIds.Contains((int)a.Id))
                .ToListAsync(cancellationToken);
        }

        dbContext.Tours.Add(model);
        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToTour(model);
    }

    public async Task<Tour?> UpdateTourAsync(
        long tourId,
        UpdateTour updateTour,
        CancellationToken cancellationToken
    )
    {
        var model = await dbContext.Tours.FirstOrDefaultAsync(
            t => t.Id == tourId,
            cancellationToken
        );

        if (model == null)
            return null;

        model.Name = updateTour.Name ?? model.Name;
        model.Description = updateTour.Description ?? model.Description;
        model.Distance = updateTour.Distance ?? model.Distance;
        model.Duration = updateTour.Duration ?? model.Duration;
        model.ElevationGain = updateTour.ElevationGain ?? model.ElevationGain;
        model.AvailableSpots = updateTour.AvailableSpots ?? model.AvailableSpots;
        model.ScheduledAt = updateTour.ScheduledAt ?? model.ScheduledAt;
        model.DestinationId = updateTour.DestinationId ?? model.DestinationId;

        if (updateTour.EquipmentIds?.Count > 0)
        {
            model.Equipment = await dbContext
                .Equipments.Where(e => updateTour.EquipmentIds.Contains(e.Id))
                .ToListAsync(cancellationToken);
        }

        if (updateTour.AttractionIds?.Count > 0)
        {
            model.Attractions = await dbContext
                .Attractions.Where(a => updateTour.AttractionIds.Contains(a.Id))
                .ToListAsync(cancellationToken);
        }

        model.UpdatedAt = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToTour(model);
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

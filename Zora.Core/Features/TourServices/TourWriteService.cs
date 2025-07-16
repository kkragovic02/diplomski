using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.Models;

namespace Zora.Core.Features.TourServices;

internal class TourWriteService(ZoraDbContext dbContext) : ITourWriteService
{
    public async Task<Tour> CreateAsync(CreateTour createTour, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;

        if (createTour.ScheduledAt <= now)
        {
            throw new InvalidOperationException("Tura ne može biti zakazana u prošlosti.");
        }

        var scheduledDate = createTour.ScheduledAt.Date;
        var nextDay = scheduledDate.AddDays(1);

        var isDateOccupied = await dbContext.Tours.AnyAsync(
            t => t.ScheduledAt >= scheduledDate && t.ScheduledAt < nextDay,
            cancellationToken
        );

        if (isDateOccupied)
        {
            throw new InvalidOperationException("Tura je već zakazana za taj datum.");
        }

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

        await AddEquipmentAndAttractionsAsync(tourModel, createTour, cancellationToken);

        dbContext.Tours.Add(tourModel);
        await dbContext.SaveChangesAsync(cancellationToken);

        return tourModel.MapToTour();
    }

    public async Task<Tour?> UpdateAsync(
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
        {
            return null;
        }

        var now = DateTimeOffset.UtcNow;
        var scheduledAt = updateTour.ScheduledAt ?? tourModel.ScheduledAt;

        if (scheduledAt <= now)
        {
            throw new InvalidOperationException("Tura ne može biti zakazana u prošlosti.");
        }

        var scheduledDate = scheduledAt.Date;
        var nextDay = scheduledDate.AddDays(1);

        var isDateOccupied = await dbContext.Tours.AnyAsync(
            t => t.Id != tourId && t.ScheduledAt >= scheduledDate && t.ScheduledAt < nextDay,
            cancellationToken
        );

        if (isDateOccupied)
        {
            throw new InvalidOperationException("Tura je već zakazana za taj datum.");
        }

        tourModel.Name = updateTour.Name ?? tourModel.Name;
        tourModel.Description = updateTour.Description ?? tourModel.Description;
        tourModel.Distance = updateTour.Distance ?? tourModel.Distance;
        tourModel.Duration = updateTour.Duration ?? tourModel.Duration;
        tourModel.ElevationGain = updateTour.ElevationGain ?? tourModel.ElevationGain;
        tourModel.AvailableSpots = updateTour.AvailableSpots ?? tourModel.AvailableSpots;
        tourModel.ScheduledAt = scheduledAt;
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

        return tourModel.MapToTour();
    }

    public async Task<bool> DeleteAsync(long tourId, CancellationToken cancellationToken)
    {
        var tour = await dbContext
            .Tours.Include(t => t.Participants)
            .Include(t => t.Equipment)
            .Include(t => t.Attractions)
            .FirstOrDefaultAsync(t => t.Id == tourId, cancellationToken);

        if (tour == null)
            return false;

        // Obriši sve CheckListItem-e povezane sa ovom turom
        var checkListItems = await dbContext
            .UserCheckLists.Where(c => c.TourId == tourId)
            .ToListAsync(cancellationToken);

        foreach (var item in checkListItems)
        {
            dbContext.Entry(item).State = EntityState.Deleted;
        }

        // Ako ne koristiš Cascade delete, obriši ručno iz join tabela:
        // (opciono) Ako koristiš many-to-many bez eksplicitnog modela:
        tour.Participants = [];
        tour.Equipment = [];
        tour.Attractions = [];

        dbContext.Tours.Remove(tour);

        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task AddEquipmentAndAttractionsAsync(
        TourModel tourModel,
        CreateTour createTour,
        CancellationToken cancellationToken
    )
    {
        var equipmentTask = Task.FromResult(new List<EquipmentModel>());
        var attractionsTask = Task.FromResult(new List<AttractionModel>());

        if (createTour.EquipmentIds.Count > 0)
        {
            equipmentTask = dbContext
                .Equipments.Where(equipment => createTour.EquipmentIds.Contains(equipment.Id))
                .ToListAsync(cancellationToken);
        }

        if (createTour.AttractionIds.Count > 0)
        {
            attractionsTask = dbContext
                .Attractions.Where(attraction => createTour.AttractionIds.Contains(attraction.Id))
                .ToListAsync(cancellationToken);
        }

        await Task.WhenAll(equipmentTask, attractionsTask);

        tourModel.Equipment = await equipmentTask;
        tourModel.Attractions = await attractionsTask;
    }
}

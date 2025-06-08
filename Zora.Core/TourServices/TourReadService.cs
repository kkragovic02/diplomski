using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.TourServices.Models;

namespace Zora.Core.TourServices;

internal class TourReadService(ZoraDbContext dbContext) : ITourReadService
{
    public async Task<Tour> GetTourByIdAsync(long tourId, CancellationToken cancellationToken)
    {
        var tour = await dbContext
            .Tours.AsNoTracking()
            .Include(t => t.Equipment)
            .Include(t => t.Attractions)
            .FirstOrDefaultAsync(t => t.Id == tourId, cancellationToken);

        return tour is null ? throw new KeyNotFoundException("Tour not found") : MapToTour(tour);
    }

    public async Task<List<Tour>> GetAllToursForUserAsync(
        long userId,
        CancellationToken cancellationToken
    )
    {
        var tours = await dbContext
            .Tours.AsNoTracking()
            .Include(t => t.Equipment)
            .Include(t => t.Attractions)
            .Where(t => t.Participants.Any(u => u.Id == userId))
            .ToListAsync(cancellationToken);

        return tours.Select(MapToTour).ToList();
    }

    private static Tour MapToTour(Database.Models.TourModel model) =>
        new()
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            Distance = model.Distance,
            Duration = model.Duration,
            ElevationGain = model.ElevationGain,
            AvailableSpots = model.AvailableSpots,
            ScheduledAt = model.ScheduledAt.DateTime,
            DestinationId = model.DestinationId,
            GuideId = model.GuideId,
            EquipmentIds = model.Equipment.Select(e => (int)e.Id).ToList(),
            AttractionIds = model.Attractions.Select(a => (int)a.Id).ToList(),
        };
}

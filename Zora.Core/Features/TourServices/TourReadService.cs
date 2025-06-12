using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.Features.TourServices.Models;

namespace Zora.Core.Features.TourServices;

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

    private static Tour MapToTour(TourModel model) =>
        new Tour(
            model.Id,
            model.Name,
            model.Description,
            model.Distance,
            model.Duration,
            model.ElevationGain,
            model.AvailableSpots,
            model.ScheduledAt.DateTime,
            model.DestinationId,
            model.GuideId,
            model.Equipment.Select(e => e.Id).ToList(),
            model.Attractions.Select(a => a.Id).ToList()
        );
}

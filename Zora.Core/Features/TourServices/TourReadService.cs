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
            .Include(tour => tour.Equipment)
            .Include(tour => tour.Attractions)
            .FirstOrDefaultAsync(tour => tour.Id == tourId, cancellationToken);

        return tour is null ? throw new KeyNotFoundException("Tour not found") : MapToTour(tour);
    }

    public async Task<List<Tour>> GetAllToursForUserAsync(
        long userId,
        CancellationToken cancellationToken
    )
    {
        var tours = await dbContext
            .Tours.AsNoTracking()
            .Include(tour => tour.Equipment)
            .Include(tour => tour.Attractions)
            .Where(tour => tour.Participants.Any(u => u.Id == userId))
            .ToListAsync(cancellationToken);

        return tours.Select(MapToTour).ToList();
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

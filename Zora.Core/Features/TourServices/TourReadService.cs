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
    public async Task<Tour> GetByIdAsync(long tourId, CancellationToken cancellationToken)
    {
        var tourModel = await dbContext
            .Tours.AsNoTracking()
            .Include(tour => tour.Equipment)
            .Include(tour => tour.Attractions)
            .FirstOrDefaultAsync(tour => tour.Id == tourId, cancellationToken);

        return tourModel is null
            ? throw new KeyNotFoundException("Tour not found")
            : MapToTour(tourModel);
    }

    public async Task<List<Tour>> GetAllForUserAsync(
        long userId,
        CancellationToken cancellationToken
    )
    {
        var tourModels = await dbContext
            .Tours.AsNoTracking()
            .Include(tour => tour.Equipment)
            .Include(tour => tour.Attractions)
            .Where(tour => tour.Participants.Any(u => u.Id == userId))
            .ToListAsync(cancellationToken);

        return tourModels.Select(MapToTour).ToList();
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

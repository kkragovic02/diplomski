using Microsoft.EntityFrameworkCore;
using Zora.Core.Database;
using Zora.Core.Models;

namespace Zora.Core.Features.TourServices;

internal class TourReadService(ZoraDbContext dbContext) : ITourReadService
{
    public async Task<Tour> GetByIdAsync(long tourId, CancellationToken cancellationToken)
    {
        var tourModel = await dbContext
            .Tours.AsNoTracking()
            .Include(tour => tour.Equipment)
            .Include(tour => tour.Attractions)
            .Include(tour => tour.Destination)
            .FirstOrDefaultAsync(tour => tour.Id == tourId, cancellationToken);

        return tourModel is null
            ? throw new KeyNotFoundException("Tour not found")
            : tourModel.MapToTour();
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
            .Include(tour => tour.Destination)
            .Where(tour => tour.Participants.Any(u => u.Id == userId))
            .ToListAsync(cancellationToken);

        return tourModels.Select(tour => tour.MapToTour()).ToList();
    }

    public async Task<TourWithGuideInfo?> GetWithGuideInfoAsync(
        long tourId,
        CancellationToken cancellationToken
    )
    {
        var tour = await dbContext
            .Tours.AsNoTracking()
            .Include(t => t.Destination)
            .Include(t => t.Guide) // assuming navigation property Guide exists
            .FirstOrDefaultAsync(t => t.Id == tourId, cancellationToken);

        if (tour is null)
        {
            return null;
        }

        return new TourWithGuideInfo(
            tour.Id,
            tour.Name,
            tour.Destination?.Name ?? "Unknown",
            tour.Distance,
            tour.ElevationGain,
            tour.AvailableSpots,
            tour.ScheduledAt.DateTime,
            tour.Guide?.Email ?? "Unknown",
            tour.Description
        );
    }

    public async Task<List<Tour>> GetAllAsync(CancellationToken cancellationToken)
    {
        var tours = await dbContext
            .Tours.Include(t => t.Equipment)
            .Include(t => t.Attractions)
            .ToListAsync(cancellationToken);

        return tours.Select(t => t.MapToTour()).ToList();
    }

    public async Task<List<TourForCalendar>> GetAllForCalendarAsync(
        CancellationToken cancellationToken
    )
    {
        var tours = await dbContext
            .Tours.Include(t => t.Destination)
            .ToListAsync(cancellationToken);

        return tours.Select(t => t.MapToCalendarDto()).ToList();
    }

    public async Task<List<Tour>> GetAllForGuideAsync(
        long guideId,
        CancellationToken cancellationToken
    )
    {
        var tourModels = await dbContext
            .Tours.AsNoTracking()
            .Include(tour => tour.Equipment)
            .Include(tour => tour.Attractions)
            .Include(tour => tour.Destination)
            .Where(tour => tour.GuideId == guideId)
            .ToListAsync(cancellationToken);

        return tourModels.Select(tour => tour.MapToTour()).ToList();
    }

    public async Task<List<Tour>> SearchAsync(
        TourSearchParameters parameters,
        CancellationToken cancellationToken
    )
    {
        var query = dbContext.Tours.Include(t => t.Attractions).AsQueryable();

        if (parameters.DateFrom.HasValue)
            query = query.Where(t => t.ScheduledAt >= parameters.DateFrom.Value);

        if (parameters.DateTo.HasValue)
            query = query.Where(t => t.ScheduledAt <= parameters.DateTo.Value);

        if (parameters.DistanceFrom.HasValue)
            query = query.Where(t => t.Distance >= parameters.DistanceFrom.Value);

        if (parameters.DistanceTo.HasValue)
            query = query.Where(t => t.Distance <= parameters.DistanceTo.Value);

        if (parameters.ElevationFrom.HasValue)
            query = query.Where(t => t.ElevationGain >= parameters.ElevationFrom.Value);

        if (parameters.ElevationTo.HasValue)
            query = query.Where(t => t.ElevationGain <= parameters.ElevationTo.Value);

        if (parameters.AttractionIds != null && parameters.AttractionIds.Count > 0)
            query = query.Where(t =>
                parameters.AttractionIds.All(id => t.Attractions.Any(a => a.Id == id))
            );

        var tours = await query.ToListAsync(cancellationToken);
        return tours.Select(t => t.MapToTour()).ToList();
    }
}

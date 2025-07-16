using Zora.Core.Models;

namespace Zora.Core.Features.TourServices;

public interface ITourReadService
{
    Task<Tour> GetByIdAsync(long tourId, CancellationToken cancellationToken);
    Task<List<Tour>> GetAllForUserAsync(long userId, CancellationToken cancellationToken);
    Task<List<Tour>> GetAllForGuideAsync(long guideId, CancellationToken cancellationToken);

    Task<TourWithGuideInfo?> GetWithGuideInfoAsync(
        long tourId,
        CancellationToken cancellationToken
    );

    Task<List<Tour>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<TourForCalendar>> GetAllForCalendarAsync(CancellationToken cancellationToken);
}

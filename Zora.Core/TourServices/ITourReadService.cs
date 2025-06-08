using Zora.Core.TourServices.Models;

namespace Zora.Core.TourServices;

public interface ITourReadService
{
    Task<Tour> GetTourByIdAsync(long tourId, CancellationToken cancellationToken);
    Task<List<Tour>> GetAllToursForUserAsync(long userId, CancellationToken cancellationToken);
}

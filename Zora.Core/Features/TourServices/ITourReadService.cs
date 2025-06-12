using Zora.Core.Features.TourServices.Models;

namespace Zora.Core.Features.TourServices;

public interface ITourReadService
{
    Task<Tour> GetTourByIdAsync(long tourId, CancellationToken cancellationToken);
    Task<List<Tour>> GetAllToursForUserAsync(long userId, CancellationToken cancellationToken);
}

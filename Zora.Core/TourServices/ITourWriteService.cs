using Zora.Core.TourServices.Models;

namespace Zora.Core.TourServices;

public interface ITourWriteService
{
    Task<Tour> CreateTourAsync(CreateTour createTour, CancellationToken cancellationToken);

    Task<Tour?> UpdateTourAsync(
        long tourId,
        UpdateTour updateTour,
        CancellationToken cancellationToken
    );

    Task<bool> DeleteTourAsync(long tourId, CancellationToken cancellationToken);
}

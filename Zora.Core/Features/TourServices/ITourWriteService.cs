using Zora.Core.Features.TourServices.Models;

namespace Zora.Core.Features.TourServices;

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

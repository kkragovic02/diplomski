using System.Threading;
using System.Threading.Tasks;
using Zora.Core.Features.TourServices.Models;

namespace Zora.Core.Features.TourServices;

public interface ITourWriteService
{
    Task<Tour> CreateAsync(CreateTour createTour, CancellationToken cancellationToken);

    Task<Tour?> UpdateAsync(
        long tourId,
        UpdateTour updateTour,
        CancellationToken cancellationToken
    );

    Task<bool> DeleteAsync(long tourId, CancellationToken cancellationToken);
}

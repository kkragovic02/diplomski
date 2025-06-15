using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zora.Core.Features.TourServices.Models;

namespace Zora.Core.Features.TourServices;

public interface ITourReadService
{
    Task<Tour> GetByIdAsync(long tourId, CancellationToken cancellationToken);
    Task<List<Tour>> GetAllForUserAsync(long userId, CancellationToken cancellationToken);
}

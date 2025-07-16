using Zora.Core.Models;

namespace Zora.Core.Features.AttractionServices;

public interface IAttractionReadService
{
    Task<IReadOnlyList<Attraction>> GetAllAsync(CancellationToken cancellationToken);

    Task<Attraction?> GetAttractionByIdAsync(long id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Attraction>> GetByTourIdAsync(
        long tourId,
        CancellationToken cancellationToken
    );
}

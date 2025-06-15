using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zora.Core.Features.AttractionServices.Models;

namespace Zora.Core.Features.AttractionServices;

public interface IAttractionReadService
{
    Task<IReadOnlyList<Attraction>> GetAllAttractionsAsync(CancellationToken cancellationToken);

    Task<Attraction?> GetAttractionByIdAsync(long id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Attraction>> GetAttractionsByTourIdAsync(
        long tourId,
        CancellationToken cancellationToken
    );
}

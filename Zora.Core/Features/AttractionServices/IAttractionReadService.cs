using Zora.Core.Features.AttractionServices.Models;

namespace Zora.Core.Features.AttractionServices;

public interface IAttractionReadService
{
    Task<IReadOnlyList<Attraction>> GetAllAttractionsAsync(CancellationToken cancellationToken);

    Task<Attraction?> GetAttractionByIdAsync(long id, CancellationToken cancellationToken);
}

using Zora.Core.AttractionServices.Models;

namespace Zora.Core.AttractionServices;

public interface IAttractionReadService
{
    Task<List<Attraction>> GetAllAttractionsAsync(CancellationToken cancellationToken = default);

    Task<Attraction?> GetAttractionByIdAsync(int id, CancellationToken cancellationToken = default);
}

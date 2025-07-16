using Zora.Core.Models;

namespace Zora.Core.Features.AttractionServices;

public interface IAttractionWriteService
{
    Task<Attraction> CreateAsync(
        CreateAttraction createAttraction,
        CancellationToken cancellationToken
    );

    Task<Attraction?> UpdateAsync(
        long attractionId,
        UpdateAttraction updateAttraction,
        CancellationToken cancellationToken
    );

    Task DeleteAsync(long id, CancellationToken cancellationToken);
}

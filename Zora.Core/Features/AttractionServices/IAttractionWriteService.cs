using Zora.Core.Features.AttractionServices.Models;

namespace Zora.Core.Features.AttractionServices;

public interface IAttractionWriteService
{
    Task<Attraction> CreateAttractionAsync(
        CreateAttraction attraction,
        CancellationToken cancellationToken
    );

    Task<Attraction?> UpdateAttractionAsync(
        long attractionId,
        UpdateAttraction updateAttraction,
        CancellationToken cancellationToken
    );

    Task DeleteAttractionAsync(long id, CancellationToken cancellationToken);
}

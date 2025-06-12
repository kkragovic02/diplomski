namespace Zora.Core.AttractionServices.Models;

public interface IAttractionWriteService
{
    Task<Attraction> CreateAttractionAsync(
        CreateAttraction attraction,
        CancellationToken cancellationToken = default
    );

    Task<Attraction?> UpdateAttractionAsync(
        UpdateAttraction attraction,
        long attractionId,
        CancellationToken cancellationToken = default
    );

    Task<bool> DeleteAttractionAsync(long id, CancellationToken cancellationToken = default);
}

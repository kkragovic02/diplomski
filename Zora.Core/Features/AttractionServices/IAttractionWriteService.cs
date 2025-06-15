using System.Threading;
using System.Threading.Tasks;
using Zora.Core.Features.AttractionServices.Models;

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

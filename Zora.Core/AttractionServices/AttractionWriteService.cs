using Microsoft.EntityFrameworkCore;
using Zora.Core.AttractionServices.Models;
using Zora.Core.Database;
using Zora.Core.Database.Models;

namespace Zora.Core.AttractionServices;

internal class AttractionWriteService(ZoraDbContext dbContext) : IAttractionWriteService
{
    public async Task<Attraction> CreateAttractionAsync(
        CreateAttraction createAttraction,
        CancellationToken cancellationToken = default
    )
    {
        var attraction = new AttractionModel { Name = createAttraction.Name };

        dbContext.Attractions.Add(attraction);
        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(attraction);
    }

    public async Task<Attraction?> UpdateAttractionAsync(
        UpdateAttraction update,
        long attractionId,
        CancellationToken cancellationToken = default
    )
    {
        var existing = await dbContext.Attractions.FirstOrDefaultAsync(
            a => a.Id == attractionId,
            cancellationToken
        );

        if (existing == null)
            return null;

        if (!string.IsNullOrWhiteSpace(update.Name))
        {
            existing.Name = update.Name;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(existing);
    }

    public async Task<bool> DeleteAttractionAsync(
        long id,
        CancellationToken cancellationToken = default
    )
    {
        var attraction = await dbContext.Attractions.FindAsync([id], cancellationToken);
        if (attraction == null)
            return false;

        dbContext.Attractions.Remove(attraction);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static Attraction MapToDto(AttractionModel model) => new(model.Id, model.Name);
}

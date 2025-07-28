using System.ComponentModel.DataAnnotations;

namespace Zora.Core.Database.Models;

public class GalleryModel
{
    public long Id { get; set; }

    public long TourId { get; set; }

    [MaxLength(100)]
    public string? FileName { get; set; } = default!;

    [MaxLength(500)]
    public string FilePath { get; set; } = default!;

    public TourModel Tour { get; set; } = default!;
}

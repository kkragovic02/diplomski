using System.ComponentModel.DataAnnotations;

namespace Zora.Core.Database.Models;

public class StoryModel
{
    public long Id { get; set; }

    public long UserId { get; set; }
    public long TourId { get; set; }

    [MaxLength(4000)]
    public string Content { get; set; } = default!;

    public List<StoryImageModel> Images { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public UserModel User { get; set; } = default!;
    public TourModel Tour { get; set; } = default!;
}

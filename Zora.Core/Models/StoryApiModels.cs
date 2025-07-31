namespace Zora.Core.Models;

public class Story
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long TourId { get; set; }
    public string Content { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<string> ImagePaths { get; set; } = new();
}

public class CreateStory
{
    public long UserId { get; set; }
    public long TourId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Content { get; set; } = default!;
}

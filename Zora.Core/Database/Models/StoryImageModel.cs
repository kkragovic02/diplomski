using System.ComponentModel.DataAnnotations;

namespace Zora.Core.Database.Models;

public class StoryImageModel
{
    public long Id { get; set; }

    public long StoryId { get; set; }

    [MaxLength(100)]
    public string FileName { get; set; } = default!;

    [MaxLength(500)]
    public string FilePath { get; set; } = default!;

    public StoryModel Story { get; set; } = default!;
}

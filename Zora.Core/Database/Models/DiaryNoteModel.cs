using System;
using System.ComponentModel.DataAnnotations;

namespace Zora.Core.Database.Models;

public class DiaryNoteModel
{
    public long Id { get; set; }

    [MaxLength(50)]
    public required string Title { get; set; }

    [MaxLength(500)]
    public required string Content { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public long UserId { get; set; }

    public UserModel? User { get; set; }

    public long TourId { get; set; }

    public TourModel? Tour { get; set; }

    public bool IsPublic { get; set; } = false;
}

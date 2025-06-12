namespace Zora.Core.Features.DiaryNoteServices.Models;

public record DiaryNote(
    long Id,
    string Title,
    string Content,
    DateTime CreatedAt,
    long UserId,
    long TourId
);

public record CreateDiaryNote(string Title, string Content, long UserId, long TourId);

public record UpdateDiaryNote(string? Title = null, string? Content = null);

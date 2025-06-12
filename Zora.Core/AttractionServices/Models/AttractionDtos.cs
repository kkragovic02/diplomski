namespace Zora.Core.AttractionServices.Models;

public record Attraction(long Id, string Name);

public record CreateAttraction(string Name);

public record UpdateAttraction(string? Name);

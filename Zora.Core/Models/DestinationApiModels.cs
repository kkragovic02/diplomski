namespace Zora.Core.Models;

public record Destination(long Id, string Name, string Description);

public record CreateDestination(string Name, string Description);

public record UpdateDestination(string? Name, string? Description);

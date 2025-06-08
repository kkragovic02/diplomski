namespace Zora.Core.DestinationServices.Models;

public record Destination(long Id, string Name, string Description);

public record CreateDestination(string Name, string Description);

public class UpdateDestination
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}

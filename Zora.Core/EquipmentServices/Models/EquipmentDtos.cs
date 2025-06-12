namespace Zora.Core.EquipmentServices.Models;

public record Equipment(long Id, string Name);

public record CreateEquipment(string Name);

public record UpdateEquipment(string? Name);

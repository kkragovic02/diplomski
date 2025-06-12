using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Zora.Core.Database.Models;

public class EquipmentModel
{
    public long Id { get; set; }

    [MaxLength(50)]
    public required string Name { get; set; }

    public ICollection<TourModel> Tours { get; set; } = new List<TourModel>();
}

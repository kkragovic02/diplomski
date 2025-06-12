using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Zora.Core.Database.Models;

public class DestinationModel
{
    public long Id { get; set; }

    [MaxLength(50)]
    public required string Name { get; set; }

    [MaxLength(500)]
    public required string Description { get; set; }

    public ICollection<TourModel> Tours { get; set; } = new List<TourModel>();
}

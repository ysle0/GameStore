using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Models {
public class Genre {
    public Guid Id { get; set; }
    [Length(5, 20)] public required string Name { get; set; }
}
}

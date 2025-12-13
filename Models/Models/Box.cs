using System.ComponentModel.DataAnnotations;
using Models.Util;

namespace Models.Models;

public class Box
{
    [Key] public required Guid Id { get; set; }

    [PositiveNumber] public required float Weight { get; set; }

    [Length(1, 50)] public string? Color { get; set; }

    [Length(1, 50)] public string? Material { get; set; }

    public Dimensions? Dimensions { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [PositiveNumber] public required int Stock { get; set; }

    [PositiveNumber] public required float Price { get; set; }
}
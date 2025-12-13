using System.ComponentModel.DataAnnotations;
using Models.Util;

namespace Models.Models;

public class Dimensions
{
    [Key] public required Guid Id { get; set; }

    [PositiveNumber] public required float Length { get; set; }

    [Required] [PositiveNumber] public required float Width { get; set; }

    [Required] [PositiveNumber] public required float Height { get; set; }

    public float Volume => Length * Width * Height;
    public float SurfaceArea => 2 * (Length * Width + Length * Height + Width * Height);
}
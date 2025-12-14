using System.ComponentModel.DataAnnotations;
using Models.Util;

namespace Models.DTOs;

public class BoxCreateDto
{
    [Required] [PositiveNumber] public required float Weight { get; set; }

    [Required] [PositiveNumber] public required int Stock { get; set; }
    [Required] [PositiveNumber] public required float Price { get; set; }
    public DimensionsDto? DimensionsDto { get; set; }
    public string? Color { get; set; }
    public string? Material { get; set; }
}
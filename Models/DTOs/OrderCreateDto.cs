using System.ComponentModel.DataAnnotations;

namespace Models.DTOs;

public class OrderCreateDto
{
    [Required] public Dictionary<Guid, int> Boxes { get; set; } = new();

    [Required] public string CustomerEmail { get; set; } = null!;
}
using System.ComponentModel.DataAnnotations;
using Models.Util;

namespace Models.DTOs;

public class CreateAddressDto
{
    [MinLength(1)] public required string StreetName { get; set; } = null!;

    [PositiveNumber] public required int HouseNumber { get; set; }

    public string? HouseNumberAddition { get; set; }

    [MinLength(1)] public required string City { get; set; } = null!;

    [MinLength(1)] public required string Country { get; set; } = null!;

    [MinLength(1)] public required string PostalCode { get; set; } = null!;
}
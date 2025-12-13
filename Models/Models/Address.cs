using System.ComponentModel.DataAnnotations;
using Models.Util;

namespace Models.Models;

public class Address
{
    [Key] public required Guid Id { get; set; }

    [Length(1, 100)] public required string StreetName { get; set; }

    [PositiveNumber] public required int HouseNumber { get; set; }

    public required string City { get; set; }

    public required string Country { get; set; }

    [PositiveNumber] public required string PostalCode { get; set; }

    public string? HouseNumberAddition { get; set; }
}
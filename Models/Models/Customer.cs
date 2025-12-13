using System.ComponentModel.DataAnnotations;

namespace Models.Models;

public class Customer
{
    [Key] [EmailAddress] public required string Email { get; set; }

    public required Address Address { get; set; }

    [Length(1, 50)] public required string FirstName { get; set; }

    [Length(1, 50)] public required string LastName { get; set; }

    public string? SimpsonImgUrl { get; set; }

    public List<Order> Orders { get; set; } = [];

    [Phone] public string? PhoneNumber { get; set; }
}
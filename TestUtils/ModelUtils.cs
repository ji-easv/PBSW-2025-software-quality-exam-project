using Models.DTOs;
using Models.Models;

namespace TestUtils;

public static class ModelUtils
{
    public static BoxUpdateDto ValidBoxUpdateDto() =>
        new()
        {
            Color = "Blue",
            Price = 29.99f,
            DimensionsDto = new DimensionsDto { Length = 15, Width = 10, Height = 8 },
            Stock = 20,
            Weight = 8,
            Material = "Wood"
        };
    
    public static BoxCreateDto ValidBoxCreateDto() =>
        new()
        {
            Color = "Green",
            Price = 24.99f,
            DimensionsDto = new DimensionsDto { Length = 12, Width = 7, Height = 5 },
            Stock = 15,
            Weight = 6,
            Material = "Cardboard"
        };

    public static Box ValidBox() =>
        new()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            Color = "Red",
            Price = 19.99f,
            Stock = 10,
            Weight = 5,
            Material = "Plastic",
            Dimensions = new Dimensions
            {
                Id = Guid.NewGuid(),
                Length = 10,
                Width = 5,
                Height = 3
            }
        };
    
    public static CreateAddressDto ValidCreateAddressDto() =>
        new()
        {
            StreetName = "742 Evergreen Terrace",
            City = "Springfield",
            PostalCode = "62704",
            Country = "USA",
            HouseNumber = 12,
            HouseNumberAddition = "A"
        };
    
    public static CreateCustomerDto ValidCreateCustomerDto () =>
        new()
        {
            FirstName = "Homer",
            LastName = "Simpson",
            Email = "homer.simpson@app.com",
            CreateAddressDto = ValidCreateAddressDto()
        };

    public static Customer ValidCustomer() =>
        new()
        {
            FirstName = "Homer",
            LastName = "Simpson",
            Email = "homer.simpson@app.com",
            SimpsonImgUrl = "/assets/img/Homer.png",
            Address = new Address
            {
                Id = Guid.NewGuid(),
                StreetName = "742 Evergreen Terrace",
                City = "Springfield",
                PostalCode = "62704",
                Country = "USA",
                HouseNumber = 12,
                HouseNumberAddition = "A"
            }
        };
    
    public static BoxCreateDto Clone(BoxCreateDto dto) => new()
    {
        Color = dto.Color,
        Price = dto.Price,
        DimensionsDto = new DimensionsDto
        {
            Length = dto.DimensionsDto.Length,
            Width = dto.DimensionsDto.Width,
            Height = dto.DimensionsDto.Height
        },
        Stock = dto.Stock,
        Weight = dto.Weight,
        Material = dto.Material
    };
    
    public static BoxUpdateDto Clone(BoxUpdateDto dto) => new()
    {
        Color = dto.Color,
        Price = dto.Price,
        DimensionsDto = new DimensionsDto
        {
            Length = dto.DimensionsDto.Length,
            Width = dto.DimensionsDto.Width,
            Height = dto.DimensionsDto.Height
        },
        Stock = dto.Stock,
        Weight = dto.Weight,
        Material = dto.Material
    };
}
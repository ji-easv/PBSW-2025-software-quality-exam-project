using Infrastructure.Interfaces;
using Models.DTOs;
using Models.Models;
using Moq;

namespace Core.UnitTests.Utils;

public static class ModelUtils
{
    public static BoxUpdateDto CreateBoxUpdateDto() =>
        new()
        {
            Color = "Blue",
            Price = 29.99f,
            DimensionsDto = new DimensionsDto { Length = 15, Width = 10, Height = 8 },
            Stock = 20,
            Weight = 8,
            Material = "Wood"
        };
    
    public static BoxCreateDto CreateBoxCreateDto() =>
        new()
        {
            Color = "Green",
            Price = 24.99f,
            DimensionsDto = new DimensionsDto { Length = 12, Width = 7, Height = 5 },
            Stock = 15,
            Weight = 6,
            Material = "Cardboard"
        };

    public static Box CreateExistingBox() =>
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
    
    public static Box InsertBoxInDb(Mock<IBoxRepository> boxRepository)
    {
        var box = new Box
        {
            Id = Guid.NewGuid(),
            Color = "Red",
            Material = "Plastic",
            Price = 19.99f,
            Stock = 10,
            Weight = 5,
            CreatedAt = DateTime.Now
        };
        boxRepository.Setup(repo => repo.GetBoxByIdAsync(box.Id)).ReturnsAsync(box);
        return box;
    }
    
    public static CreateAddressDto CreateAddressDto() =>
        new()
        {
            StreetName = "742 Evergreen Terrace",
            City = "Springfield",
            PostalCode = "62704",
            Country = "USA",
            HouseNumber = 12,
            HouseNumberAddition = "A"
        };
    
    public static CreateCustomerDto CreateCustomerDto () =>
        new()
        {
            FirstName = "Homer",
            LastName = "Simpson",
            Email = "homer.simpson@app.com",
            CreateAddressDto = CreateAddressDto()
        };

    public static Customer CreateCustomer() =>
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
}
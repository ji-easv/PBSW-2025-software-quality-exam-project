using AutoMapper;
using Core.Mapping;
using Models.DTOs;
using Models.Models;

namespace Core.UnitTests.Mapping;

public class MappingProfileTest
{
    [Fact]
    public void MappingProfile_ConfigurationIsValid()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        
        configuration.AssertConfigurationIsValid();
    }
    
    [Fact]
    public void MappingProfile_MapsBoxCreateDtoToBox()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        var mapper = config.CreateMapper(); 
        
        var boxCreateDto = new BoxCreateDto
        {
            Color = "Red",
            Price = 19.99f,
            DimensionsDto = new DimensionsDto
            {
                Length = 10,
                Width = 5,
                Height = 3
            },
            Stock = 10,
            Weight = 5,
            Material = "Plastic"
        };
        
        var box = mapper.Map<Box>(boxCreateDto);
        Assert.Equal(boxCreateDto.Color, box.Color);
        Assert.Equal(boxCreateDto.Price, box.Price);
        Assert.Equal(boxCreateDto.Stock, box.Stock);
        Assert.Equal(boxCreateDto.Weight, box.Weight);
        Assert.Equal(boxCreateDto.Material, box.Material);
        Assert.NotNull(box.Dimensions);
        Assert.Equal(boxCreateDto.DimensionsDto!.Length, box.Dimensions!.Length);
        Assert.Equal(boxCreateDto.DimensionsDto.Width, box.Dimensions.Width);
        Assert.Equal(boxCreateDto.DimensionsDto.Height, box.Dimensions.Height);
    }

    [Fact]
    public void MappingProfile_MapsBoxUpdateDtoToBox()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        var mapper = config.CreateMapper();
        var boxUpdateDto = new BoxUpdateDto
        {
            Color = "Blue",
            Price = 29.99f,
            DimensionsDto = new DimensionsDto
            {
                Length = 15,
                Width = 10,
                Height = 8
            },
            Stock = 20,
            Weight = 8,
            Material = "Wood"
        };
        
        var existingBox = new Box
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
        
        var box = mapper.Map(boxUpdateDto, existingBox, opts =>
        {
            opts.Items["Id"] = existingBox.Id;
            opts.Items["CreatedAt"] = existingBox.CreatedAt;
        });
        
        Assert.Equal(existingBox.Id, box.Id);
        Assert.Equal(existingBox.CreatedAt, box.CreatedAt);
        Assert.Equal(boxUpdateDto.Color, box.Color);
        Assert.Equal(boxUpdateDto.Price, box.Price);
        Assert.Equal(boxUpdateDto.Stock, box.Stock);
        Assert.Equal(boxUpdateDto.Weight, box.Weight);
        Assert.Equal(boxUpdateDto.Material, box.Material);
        Assert.NotNull(box.Dimensions);
        Assert.Equal(boxUpdateDto.DimensionsDto!.Length, box.Dimensions!.Length);
        Assert.Equal(boxUpdateDto.DimensionsDto.Width, box.Dimensions.Width);
        Assert.Equal(boxUpdateDto.DimensionsDto.Height, box.Dimensions.Height);
    }
}
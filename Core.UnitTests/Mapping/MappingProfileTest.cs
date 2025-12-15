using AutoMapper;
using Core.Mapping;
using Core.UnitTests.Utils;
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
        var boxCreateDto = BoxUtils.CreateBoxCreateDto();
        
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

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MappingProfile_MapsBoxUpdateDtoToBox(bool useOpts)
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        var mapper = config.CreateMapper();

        var boxUpdateDto = BoxUtils.CreateBoxUpdateDto();
        var existingBox = BoxUtils.CreateExistingBox();

        Box box;
        if (useOpts)
        {
            box = mapper.Map(boxUpdateDto, existingBox, opts =>
            {
                opts.Items["Id"] = existingBox.Id;
                opts.Items["CreatedAt"] = existingBox.CreatedAt;
            });
        }
        else
        {
            box = mapper.Map(boxUpdateDto, existingBox);
        }

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
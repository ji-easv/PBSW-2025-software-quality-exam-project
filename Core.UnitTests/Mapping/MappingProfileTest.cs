using AutoMapper;
using Core.Mapping;
using Models.DTOs;
using Models.Models;
using TestUtils;

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
        var boxCreateDto = ModelUtils.ValidBoxCreateDto();
        
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

        var boxUpdateDto = ModelUtils.ValidBoxUpdateDto();
        var existingBox = ModelUtils.ValidBox();
        var dimensionsId = existingBox.Dimensions!.Id;

        Box box;
        if (useOpts)
        {
            box = mapper.Map(boxUpdateDto, existingBox, opts =>
            {
                opts.Items["Id"] = existingBox.Id;
                opts.Items["CreatedAt"] = existingBox.CreatedAt;
                opts.Items["DimensionsId"] = existingBox.Dimensions!.Id;
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
        Assert.Equal(dimensionsId, box.Dimensions.Id);
        Assert.Equal(boxUpdateDto.DimensionsDto!.Length, box.Dimensions!.Length);
        Assert.Equal(boxUpdateDto.DimensionsDto.Width, box.Dimensions.Width);
        Assert.Equal(boxUpdateDto.DimensionsDto.Height, box.Dimensions.Height);
    }
    
    [Fact]
    public void CreateAddressDto_MapsToAddress()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        var mapper = config.CreateMapper();

        var createAddressDto = ModelUtils.ValidCreateAddressDto();

        var address = mapper.Map<Address>(createAddressDto);
        Assert.Equal(createAddressDto.StreetName, address.StreetName);
        Assert.Equal(createAddressDto.City, address.City);
        Assert.Equal(createAddressDto.PostalCode, address.PostalCode);
        Assert.Equal(createAddressDto.Country, address.Country);
        Assert.Equal(createAddressDto.HouseNumber, address.HouseNumber);
        Assert.Equal(createAddressDto.HouseNumberAddition, address.HouseNumberAddition);
        Assert.NotEqual(Guid.Empty, address.Id);
    }

    [Fact]
    public void CreateCustomerDto_MapsToCustomer()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        var mapper = config.CreateMapper();

        var createCustomerDto = ModelUtils.ValidCreateCustomerDto();
        var customer = mapper.Map<Customer>(createCustomerDto);
        Assert.Equal(createCustomerDto.FirstName, customer.FirstName);
        Assert.Equal(createCustomerDto.LastName, customer.LastName);
        Assert.Equal(createCustomerDto.Email, customer.Email);
        Assert.NotNull(customer.Address);
        Assert.NotNull(customer.SimpsonImgUrl);
    }

    [Fact]
    public void OrderCreateDto_MapsToOrder_WithoutOptsFails()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        var mapper = config.CreateMapper();

        var orderCreateDto = new OrderCreateDto
        {
            Boxes = new Dictionary<Guid, int>
            {
                { Guid.NewGuid(), 2 },
                { Guid.NewGuid(), 3 }
            }
        };

        Assert.Throws<ArgumentException>(() => mapper.Map<Order>(orderCreateDto));
    }

    [Fact]
    public void OrderCreateDto_MapsToOrder_WithOptsSucceeds()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        var mapper = config.CreateMapper();

        var existingBoxes = new List<Box>
        {
            ModelUtils.ValidBox(),
            ModelUtils.ValidBox()
        };
        
        var orderCreateDto = new OrderCreateDto
        {
            Boxes = new Dictionary<Guid, int>
            {
                { existingBoxes[0].Id, 2 },
                { existingBoxes[1].Id, 3 }
            }
        };
        
        var order = mapper.Map<Order>(orderCreateDto, opts =>
        {
            opts.Items["Customer"] = ModelUtils.ValidCustomer();
            opts.Items["Boxes"] = existingBoxes;
        });

        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.Equal(5, order.TotalBoxes);
        Assert.Equal(ShippingStatus.Received, order.ShippingStatus);
        Assert.NotNull(order.Customer);
        Assert.NotNull(order.Boxes);
        Assert.Equal(Math.Round(99.95, 2), Math.Round(order.TotalPrice, 2));
    }
}
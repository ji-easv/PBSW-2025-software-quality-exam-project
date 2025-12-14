using AutoMapper;
using Models.DTOs;
using Models.Models;

namespace Core.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BoxCreateDto, Box>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Dimensions,
                opt => opt
                    .MapFrom(src => new Dimensions
                    {
                        Id = Guid.NewGuid(),
                        Length = src.DimensionsDto!.Length,
                        Width = src.DimensionsDto.Width,
                        Height = src.DimensionsDto.Height
                    }))
            .ReverseMap();

        CreateMap<BoxUpdateDto, Box>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Dimensions,
                opt => opt
                    .MapFrom(src => new Dimensions
                    {
                        Id = Guid.NewGuid(),
                        Length = src.DimensionsDto!.Length,
                        Width = src.DimensionsDto.Width,
                        Height = src.DimensionsDto.Height
                    }))
            .AfterMap((src, dest, ctx) =>
            {
                dest.Id = (Guid)ctx.Items["Id"];
                dest.CreatedAt = (DateTime)ctx.Items["CreatedAt"];
            });

        CreateMap<Dimensions, DimensionsDto>();

        CreateMap<CreateAddressDto, Address>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ReverseMap();

        CreateMap<CreateCustomerDto, Customer>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.CreateAddressDto))
            .ForMember(dest => dest.SimpsonImgUrl, opt => opt.MapFrom(_ => GetRandomSimpsonImage()));
        
        CreateMap<OrderCreateDto, Order>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.ShippingStatus, opt => opt.MapFrom(_ => ShippingStatus.Received))
            .ForMember(dest => dest.TotalBoxes,
                opt => opt.MapFrom(src => src.Boxes.Values.Sum()))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Boxes.Values.Sum()))
            .ForMember(dest => dest.Boxes, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .AfterMap((src, dest, ctx) =>
            {
                dest.Boxes = (List<Box>)ctx.Items["Boxes"];
                dest.Customer = (Customer)ctx.Items["Customer"];
                dest.TotalPrice = ((List<Box>)ctx.Items["Boxes"]).Sum(b => b.Price * src.Boxes[b.Id]);
            });
    }

    private static string GetRandomSimpsonImage()
    {
        Random random = new();
        var simpsons = new List<string>
        {
            "/assets/img/Abe.png",
            "/assets/img/Burns.png",
            "/assets/img/Moe.png",
            "/assets/img/Homer.png"
        };
        return simpsons[random.Next(simpsons.Count)];
    }
}
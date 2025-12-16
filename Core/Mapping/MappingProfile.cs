using AutoMapper;
using Models.DTOs;
using Models.Models;

namespace Core.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateBoxMappers();

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
            .ForMember(dest => dest.Boxes, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
            .AfterMap((src, dest, ctx) =>
            {
                if (!ctx.TryGetItems(out var items))
                    throw new ArgumentException("Mapping context items are required but were not provided.");

                if (!items.TryGetValue("Boxes", out var boxesObj) || boxesObj is not List<Box> boxes)
                    throw new ArgumentException("'Boxes' item (List<Box>) is required in mapping context.");

                dest.Boxes = boxes;
                dest.TotalPrice = boxes.Sum(b => b.Price * src.Boxes.GetValueOrDefault(b.Id, 0));

                if (!items.TryGetValue("Customer", out var customerObj) || customerObj is not Customer customer)
                    throw new ArgumentException("'Customer' item (Customer) is required in mapping context.");

                dest.Customer = customer;
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

    private void CreateBoxMappers()
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
                opt => opt.Ignore())
            .AfterMap((src, dest, ctx) =>
            {
                if (ctx.TryGetItems(out var items))
                {
                    if (items.TryGetValue("Id", out var id)) dest.Id = (Guid)id!;
                    if (items.TryGetValue("CreatedAt", out var createdAt)) dest.CreatedAt = (DateTime)createdAt!;
                    if (items.TryGetValue("DimensionsId", out var dimensionsId))
                    {
                        dest.Dimensions = new Dimensions
                        {
                            Id = (Guid)dimensionsId!,
                            Length = dest.Dimensions?.Length ?? 0,
                            Width = dest.Dimensions?.Width ?? 0,
                            Height = dest.Dimensions?.Height ?? 0
                        };
                    }
                }

                if (src.DimensionsDto != null && dest.Dimensions != null)
                {
                    dest.Dimensions.Length = src.DimensionsDto.Length;
                    dest.Dimensions.Width = src.DimensionsDto.Width;
                    dest.Dimensions.Height = src.DimensionsDto.Height;
                }
            });
    }
}
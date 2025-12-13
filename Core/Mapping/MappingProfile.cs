using AutoMapper;
using Models.DTOs;
using Models.Models;

namespace Core.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BoxCreateDto, Box>()
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

        CreateMap<Dimensions, DimensionsDto>();

        CreateMap<CreateAddressDto, Address>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ReverseMap();

        CreateMap<CreateCustomerDto, Customer>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.CreateAddressDto))
            .ForMember(dest => dest.SimpsonImgUrl, opt => opt.MapFrom(_ => GetRandomSimpsonImage()));
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
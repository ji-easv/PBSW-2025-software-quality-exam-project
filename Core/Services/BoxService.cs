using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Core.Services.Interfaces;
using Infrastructure.Interfaces;
using Models;
using Models.DTOs;
using Models.Exceptions;
using Models.Models;
using Models.Util;

namespace Core.Services;

public class BoxService(IBoxRepository boxRepository, IMapper mapper) : IBoxService
{
    private readonly List<string> _supportedColors =
    [
        "red", "blue", "green", "yellow", "black", "white", "brown", "grey", "orange", "purple", "pink", "gold",
        "silver", "bronze", "copper"
    ];

    private readonly List<string> _supportedMaterials =
        ["cardboard", "plastic", "wood", "metal"];


    public async Task<IEnumerable<Box>> GetBoxesForOderAsync(Dictionary<Guid, int> boxQuantities)
    {
        var boxIds = boxQuantities.Keys;
        var boxes = (await boxRepository.GetBoxesByIdsAsync(boxIds)).ToList();

        // Check if all boxes exist
        if (boxes.Count != boxIds.Count)
        {
            var foundBoxIds = boxes.Select(b => b.Id);
            var missingBoxIds = boxIds.Except(foundBoxIds);
            throw new NotFoundException($"Boxes with ids {string.Join(", ", missingBoxIds)} not found");
        }

        // Check if each box is in the required stock
        foreach (var box in boxes)
        {
            var requiredQuantity = boxQuantities[box.Id];
            if (box.Stock < requiredQuantity)
                throw new ValidationException(
                    $"Box with id {box.Id} does not have enough stock. Required: {requiredQuantity}, Available: {box.Stock}");
        }

        return boxes;
    }

    public async Task<SearchBoxResult> SearchBoxesAsync(BoxParameters boxParameters)
    {
        return await boxRepository.SearchBoxesAsync(boxParameters);
    }

    public async Task<Box> GetBoxByIdAsync(Guid boxId)
    {
        var box = await boxRepository.GetBoxByIdAsync(boxId);
        return box ?? throw new NotFoundException($"Box with id {boxId} not found");
    }

    public Task<Box> CreateBoxAsync(BoxCreateDto boxCreateDto)
    {
        var box = mapper.Map<Box>(boxCreateDto);

        if (box.Color is not null && !_supportedColors.Contains(box.Color.ToLower()))
            throw new ValidationException($"Color '{box.Color}' is not supported.");

        if (box.Material is not null && !_supportedMaterials.Contains(box.Material.ToLower()))
            throw new ValidationException($"Material '{box.Material}' is not supported.");

        box.CreatedAt = DateTime.UtcNow;
        return boxRepository.CreateBoxAsync(box);
    }

    public async Task<Box> UpdateBoxAsync(Guid id, BoxUpdateDto boxUpdateDto)
    {
        if (boxUpdateDto.Color is not null && !_supportedColors.Contains(boxUpdateDto.Color.ToLower()))
            throw new ValidationException($"Color '{boxUpdateDto.Color}' is not supported.");

        if (boxUpdateDto.Material is not null && !_supportedMaterials.Contains(boxUpdateDto.Material.ToLower()))
            throw new ValidationException($"Material '{boxUpdateDto.Material}' is not supported.");
        
        var box = await GetBoxByIdAsync(id);
        box = mapper.Map(boxUpdateDto, box);
        box.UpdatedAt = DateTime.UtcNow;
        return await boxRepository.UpdateBoxAsync(box);
    }

    public async Task DeleteBoxAsync(Guid boxId)
    {
        var box = await GetBoxByIdAsync(boxId);
        await boxRepository.DeleteBoxAsync(box);
    }
}
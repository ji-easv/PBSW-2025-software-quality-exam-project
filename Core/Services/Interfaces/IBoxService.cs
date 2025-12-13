using Models;
using Models.DTOs;
using Models.Models;
using Models.Util;

namespace Core.Services.Interfaces;

public interface IBoxService
{
    Task<IEnumerable<Box>> GetBoxesForOderAsync(Dictionary<Guid, int> boxQuantities);
    Task<SearchBoxResult> SearchBoxesAsync(BoxParameters boxParameters);
    Task<Box> GetBoxByIdAsync(Guid boxId);
    Task<Box> CreateBoxAsync(BoxCreateDto boxCreateDto);
    Task<Box> UpdateBoxAsync(Guid id, BoxUpdateDto boxUpdateDto);
    Task DeleteBoxAsync(Guid boxId);
}
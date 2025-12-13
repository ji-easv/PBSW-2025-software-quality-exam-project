using Models;
using Models.Models;
using Models.Util;

namespace Infrastructure.Interfaces;

public interface IBoxRepository
{
    Task<IEnumerable<Box>> GetBoxesByIdsAsync(IEnumerable<Guid> boxIds);
    Task<Box?> GetBoxByIdAsync(Guid boxId);
    Task<Box> CreateBoxAsync(Box box);
    Task<Box> UpdateBoxAsync(Box box);
    Task DeleteBoxAsync(Box box);
    Task<SearchBoxResult> SearchBoxesAsync(BoxParameters boxParameters);
}
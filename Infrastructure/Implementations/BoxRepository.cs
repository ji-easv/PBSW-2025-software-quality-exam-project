using System.Linq.Expressions;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.Util;

namespace Infrastructure.Implementations;

public class BoxRepository(ApplicationDbContext applicationDbContext) : IBoxRepository
{
    public async Task<IEnumerable<Box>> GetBoxesByIdsAsync(IEnumerable<Guid> boxIds)
    {
        return await applicationDbContext.Boxes
            .Include(b => b.Dimensions)
            .Where(box => boxIds.Contains(box.Id))
            .ToListAsync();
    }

    public async Task<Box?> GetBoxByIdAsync(Guid boxId)
    {
        return await applicationDbContext.Boxes
            .Include(b => b.Dimensions)
            .FirstOrDefaultAsync(b => b.Id == boxId);
    }

    public async Task<Box> CreateBoxAsync(Box box)
    {
        applicationDbContext.Boxes.Add(box);
        await applicationDbContext.SaveChangesAsync();
        return box;
    }

    public async Task<Box> UpdateBoxAsync(Box box)
    {
        applicationDbContext.Boxes.Update(box);
        await applicationDbContext.SaveChangesAsync();
        return box;
    }

    public async Task DeleteBoxAsync(Box box)
    {
        applicationDbContext.Boxes.Remove(box);
        await applicationDbContext.SaveChangesAsync();
    }

    public async Task<SearchBoxResult> SearchBoxesAsync(BoxParameters boxParameters)
    {
        var searchQuery = applicationDbContext.Boxes.Include(b => b.Dimensions).AsQueryable();
        searchQuery = ApplySearchFilters(searchQuery, boxParameters);
        searchQuery = ApplySorting(searchQuery, boxParameters);

        // Apply pagination
        searchQuery = searchQuery
            .Skip((boxParameters.CurrentPage - 1) * boxParameters.BoxesPerPage)
            .Take(boxParameters.BoxesPerPage);

        var totalBoxes = await applicationDbContext.Boxes.CountAsync();
        var totalPages = (int)Math.Ceiling(totalBoxes / (double)boxParameters.BoxesPerPage);
        var boxes = await searchQuery
            .ToListAsync();
        return new SearchBoxResult
        {
            Boxes = boxes,
            CurrentPage = boxParameters.CurrentPage,
            TotalPages = totalPages,
            BoxesPerPage = boxParameters.BoxesPerPage,
            TotalBoxes = totalBoxes
        };
    }

    private IQueryable<Box> ApplySearchFilters(IQueryable<Box> searchQuery, BoxParameters boxParameters)
    {
        // Apply search term
        if (!string.IsNullOrWhiteSpace(boxParameters.SearchTerm))
            searchQuery = searchQuery.Where(b =>
                (b.Color != null && b.Color.Contains(boxParameters.SearchTerm)) ||
                (b.Material != null && b.Material.Contains(boxParameters.SearchTerm)));

        // Apply filters
        if (string.IsNullOrWhiteSpace(boxParameters.Filters)) return searchQuery;

        var filters = boxParameters.GetFilters();
        foreach (var filter in filters)
        {
            var values = filter.Value.Split(',');

            searchQuery = filter.Key switch
            {
                FilterTypes.Color => searchQuery.Where(b => values.Contains(b.Color)),
                FilterTypes.Material => searchQuery.Where(b => values.Contains(b.Material)),
                FilterTypes.Weight => ApplyRangeFilter(searchQuery, b => b.Weight, filter.Value),
                FilterTypes.Length => ApplyRangeFilter(searchQuery, b => b.Dimensions != null ? b.Dimensions.Length : 0,
                    filter.Value),
                FilterTypes.Width => ApplyRangeFilter(searchQuery, b => b.Dimensions != null ? b.Dimensions.Width : 0,
                    filter.Value),
                FilterTypes.Height => ApplyRangeFilter(searchQuery, b => b.Dimensions != null ? b.Dimensions.Height : 0,
                    filter.Value),
                FilterTypes.Price => ApplyRangeFilter(searchQuery, b => b.Price, filter.Value),
                FilterTypes.Stock => ApplyRangeFilter(searchQuery, b => b.Stock, filter.Value),
                _ => searchQuery
            };
        }

        return searchQuery;
    }

    private IQueryable<Box> ApplySorting(IQueryable<Box> query, BoxParameters boxParameters)
    {
        var sortBy = boxParameters.SortBy;
        var descending = boxParameters.Descending == true;

        if (string.IsNullOrWhiteSpace(sortBy)) return query;

        query = boxParameters.SortBy.ToLower() switch
        {
            "weight" => boxParameters.Descending == true
                ? query.OrderByDescending(b => b.Weight)
                : query.OrderBy(b => b.Weight),
            _ => query.OrderByDescending(b => b.CreatedAt)
        };
        return query;
    }

    private IQueryable<Box> ApplyRangeFilter<T>(
        IQueryable<Box> query,
        Expression<Func<Box, T>> propertySelector,
        string rangeString) where T : struct, IComparable<T>
    {
        var range = rangeString.Split('-');
        if (range.Length != 2) return query;

        var minValue = (T?)Convert.ChangeType(range[0], typeof(T));
        var maxValue = (T?)Convert.ChangeType(range[1], typeof(T));

        var parameter = Expression.Parameter(typeof(Box), "b");
        var property = Expression.Invoke(propertySelector, parameter);

        var minConstant = Expression.Constant(minValue.Value, typeof(T));
        var maxConstant = Expression.Constant(maxValue.Value, typeof(T));

        var greaterThan = Expression.GreaterThanOrEqual(property, minConstant);
        var lessThan = Expression.LessThanOrEqual(property, maxConstant);
        var combined = Expression.AndAlso(greaterThan, lessThan);

        var lambda = Expression.Lambda<Func<Box, bool>>(combined, parameter);

        return query.Where(lambda);
    }
}
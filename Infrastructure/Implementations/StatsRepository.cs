using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class StatsRepository(ApplicationDbContext dbContext) : IStatsRepository
{
    public async Task<Dictionary<int, int>> GetAggregatedMonthlyStatsAsync()
    {
        var twelveMonthsAgo = DateTime.UtcNow.AddMonths(-12);

        var stats = await dbContext.Orders
            .Where(o => o.CreatedAt > twelveMonthsAgo)
            .SelectMany(o => o.Boxes.Select(b => o.CreatedAt.Month))
            .GroupBy(month => month)
            .Select(g => new
            {
                Month = g.Key,
                Boxes = g.Count()
            })
            .ToListAsync();
    
        // Initialize dictionary with 0 for all months (0-indexed)
        var statsDictionary = Enumerable.Range(0, 12).ToDictionary(i => i, i => 0);
    
        // Populate with actual data (convert to 0-indexed)
        foreach (var stat in stats)
        {
            statsDictionary[stat.Month - 1] = stat.Boxes;
        }
    
        return statsDictionary;
    }
}
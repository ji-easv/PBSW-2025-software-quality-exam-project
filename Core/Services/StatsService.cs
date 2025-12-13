using Core.Services.Interfaces;
using Infrastructure.Interfaces;

namespace Core.Services;

public class StatsService(IStatsRepository statsRepository) : IStatsService
{
    public async Task<Dictionary<int, int>> GetAggregatedMonthlyStatsAsync()
    {
        return await statsRepository.GetAggregatedMonthlyStatsAsync();
    }
}
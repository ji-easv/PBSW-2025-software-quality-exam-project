namespace Infrastructure.Interfaces;

public interface IStatsRepository
{
    Task<Dictionary<int, int>> GetAggregatedMonthlyStatsAsync();
}
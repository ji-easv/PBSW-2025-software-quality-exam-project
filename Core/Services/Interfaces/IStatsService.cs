namespace Core.Services.Interfaces;

public interface IStatsService
{
    Task<Dictionary<int, int>> GetAggregatedMonthlyStatsAsync();
}
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoxFactoryAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class StatsController(IStatsService statsService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Dictionary<int,int>>> Get()
    {
        return Ok(await statsService.GetAggregatedMonthlyStatsAsync());
    }
}
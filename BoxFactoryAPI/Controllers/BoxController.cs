using Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Models.Models;
using Models.Util;

namespace BoxFactoryAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BoxController(IBoxService boxService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<SearchBoxResult>> SearchBoxesAsync([FromQuery] BoxParameters boxParameters)
    {
        return Ok(await boxService.SearchBoxesAsync(boxParameters));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Box>> Get([FromRoute] Guid id)
    {
        return Ok(await boxService.GetBoxByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<Box>> Create([FromBody] BoxCreateDto boxCreateDto)
    {
        return Ok(await boxService.CreateBoxAsync(boxCreateDto));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Box>> Update([FromRoute] Guid id, [FromBody] BoxUpdateDto boxUpdateDto)
    {
        return Ok(await boxService.UpdateBoxAsync(id, boxUpdateDto));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await boxService.DeleteBoxAsync(id);
        return Ok();
    }
}
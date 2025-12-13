using Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.Models;

namespace BoxFactoryAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Order>> Create([FromBody] OrderCreateDto orderCreateDto)
    {
        return await orderService.CreateAsync(orderCreateDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> Get()
    {
        return Ok(await orderService.GetAllAsync());
    }

    [HttpGet("{status}")]
    public async Task<ActionResult<IEnumerable<Order>>> GetByStatus(ShippingStatus status)
    {
        return Ok(await orderService.GetByStatusAsync(status));
    }

    [HttpGet("latest")]
    public async Task<ActionResult<IEnumerable<Order>>> GetRecent()
    {
        return Ok(await orderService.GetLatestAsync());
    }

    [HttpGet("orders-count")]
    public async Task<ActionResult<int>> GetTotalOrders()
    {
        return Ok(await orderService.GetTotalOrdersAsync());
    }

    [HttpGet("revenue")]
    public async Task<ActionResult<float>> GetTotalRevenue()
    {
        return Ok(await orderService.GetTotalRevenueAsync());
    }

    [HttpGet("boxes-sold")]
    public async Task<ActionResult<int>> GetTotalBoxesSold()
    {
        return Ok(await orderService.GetTotalBoxesSoldAsync());
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<Order>> UpdateStatus(Guid id, [FromQuery] ShippingStatus newStatus)
    {
        var order = await orderService.UpdateStatusAsync(id, newStatus);
        return Ok(order);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await orderService.DeleteAsync(id);
        return Ok();
    }
}
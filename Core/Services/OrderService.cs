using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Core.Services.Interfaces;
using Infrastructure.Interfaces;
using Models.DTOs;
using Models.Exceptions;
using Models.Models;

namespace Core.Services;

public class OrderService(
    IOrderRepository orderRepository,
    ICustomerService customerService,
    IBoxService boxService,
    IMapper mapper
) : IOrderService
{
    public async Task<Order> CreateAsync(OrderCreateDto orderCreateDto)
    {
        if (orderCreateDto.Boxes.Count == 0) throw new ValidationException("No boxes in order.");

        var customer = await customerService.GetCustomerByEmailAsync(orderCreateDto.CustomerEmail);
        var boxes = (await boxService.GetBoxesForOderAsync(orderCreateDto.Boxes)).ToList();

        var order = mapper.Map<Order>(orderCreateDto, opt =>
        {
            opt.Items["Boxes"] = boxes;
            opt.Items["Customer"] = customer;
        });

        return await orderRepository.CreateOrderAsync(order);
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await orderRepository.GetAllOrdersAsync();
    }

    public async Task<IEnumerable<Order>> GetByStatusAsync(ShippingStatus status)
    {
        return await orderRepository.GetByStatusAsync(status);
    }

    public async Task<IEnumerable<Order>> GetLatestAsync()
    {
        return await orderRepository.GetLatestAsync();
    }

    public async Task<int> GetTotalOrdersAsync()
    {
        return await orderRepository.GetTotalOrdersAsync();
    }

    public async Task<float> GetTotalRevenueAsync()
    {
        return await orderRepository.GetTotalRevenueAsync();
    }

    public async Task<int> GetTotalBoxesSoldAsync()
    {
        return await orderRepository.GetTotalBoxesSoldAsync();
    }

    public async Task<Order> UpdateStatusAsync(Guid orderId, ShippingStatus newStatus)
    {
        var order = await GetOrderByIdAsync(orderId);

        order.ShippingStatus = newStatus;
        order.UpdatedAt = DateTime.UtcNow;

        return await orderRepository.UpdateOrderAsync(order);
    }

    public async Task DeleteAsync(Guid orderId)
    {
        var order = await GetOrderByIdAsync(orderId);
        await orderRepository.DeleteOrderAsync(order);
    }

    private async Task<Order> GetOrderByIdAsync(Guid orderId)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);
        return order ?? throw new NotFoundException("Order not found.");
    }
}
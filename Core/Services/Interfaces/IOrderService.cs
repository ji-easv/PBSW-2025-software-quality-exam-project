using Models.DTOs;
using Models.Models;

namespace Core.Services.Interfaces;

public interface IOrderService
{
    public Task<Order> CreateAsync(OrderCreateDto orderCreateDto);
    public Task<IEnumerable<Order>> GetAllAsync();
    public Task<IEnumerable<Order>> GetByStatusAsync(ShippingStatus status);
    public Task<IEnumerable<Order>> GetLatestAsync();
    public Task<int> GetTotalOrdersAsync();
    public Task<float> GetTotalRevenueAsync();
    public Task<int> GetTotalBoxesSoldAsync();
    public Task<Order> UpdateStatusAsync(Guid orderId, ShippingStatus newStatus);
    public Task DeleteAsync(Guid orderId);
}
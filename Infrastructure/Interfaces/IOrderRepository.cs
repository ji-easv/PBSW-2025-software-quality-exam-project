using Models.Models;

namespace Infrastructure.Interfaces;

public interface IOrderRepository
{
    Task<Order> CreateOrderAsync(Order order);
    Task<Order?> GetOrderByIdAsync(Guid orderId);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<IEnumerable<Order>> GetByStatusAsync(ShippingStatus status);
    Task<IEnumerable<Order>> GetLatestAsync(int take = 10);
    Task<Order> UpdateOrderAsync(Order order);
    Task DeleteOrderAsync(Order order);
    
    Task<int> GetTotalOrdersAsync();
    Task<float> GetTotalRevenueAsync();
    Task<int> GetTotalBoxesSoldAsync();
}
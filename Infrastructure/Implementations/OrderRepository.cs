using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Infrastructure.Implementations;

public class OrderRepository(ApplicationDbContext dbContext) : IOrderRepository
{
    public async Task<Order> CreateOrderAsync(Order order)
    {
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();
        return order;
    }

    public async Task<Order?> GetOrderByIdAsync(Guid orderId)
    {
        return await dbContext.Orders.FindAsync(orderId);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await dbContext.Orders.ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetByStatusAsync(ShippingStatus status)
    {
        return await dbContext.Orders.Where(o => o.ShippingStatus == status).ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetLatestAsync(int take = 10)
    {
        return await dbContext.Orders.OrderByDescending(o => o.CreatedAt).Take(take).ToListAsync();
    }

    public async Task<Order> UpdateOrderAsync(Order order)
    {
        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync();
        return order;
    }

    public Task DeleteOrderAsync(Order order)
    {
        dbContext.Orders.Remove(order);
        return dbContext.SaveChangesAsync();
    }

    public async Task<int> GetTotalOrdersAsync()
    {
        return await dbContext.Orders.CountAsync();
    }

    public async Task<float> GetTotalRevenueAsync()
    {
        return await dbContext.Orders.SumAsync(o => o.TotalPrice);
    }

    public async Task<int> GetTotalBoxesSoldAsync()
    {
        return await dbContext.Orders.SumAsync(o => o.TotalBoxes);
    }
}
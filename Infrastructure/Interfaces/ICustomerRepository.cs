using Models.Models;

namespace Infrastructure.Interfaces;

public interface ICustomerRepository
{
    public Task<Customer?> GetCustomerByEmailAsync(string email);
    public Task<Customer> CreateCustomerAsync(Customer customer);
}
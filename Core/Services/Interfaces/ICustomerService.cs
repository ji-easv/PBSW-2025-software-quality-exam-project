using Models.DTOs;
using Models.Models;

namespace Core.Services.Interfaces;

public interface ICustomerService
{
    public Task<Customer> GetCustomerByEmailAsync(string email);
    public Task<Customer> CreateCustomerAsync(CreateCustomerDto customerDto);
}
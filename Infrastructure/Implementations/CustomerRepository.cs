using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Infrastructure.Implementations;

public class CustomerRepository(ApplicationDbContext dbContext) : ICustomerRepository
{
    public async Task<Customer?> GetCustomerByEmailAsync(string email)
    {
        return await dbContext.Customers
            .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
    }

    public Task<Customer> CreateCustomerAsync(Customer customer)
    {
        throw new NotImplementedException();
    }
}
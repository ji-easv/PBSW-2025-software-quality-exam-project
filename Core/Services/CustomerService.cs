using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Core.Services.Interfaces;
using Infrastructure.Interfaces;
using Models.DTOs;
using Models.Exceptions;
using Models.Models;

namespace Core.Services;

public class CustomerService(ICustomerRepository customerRepository, IMapper mapper) : ICustomerService
{
    public async Task<Customer> GetCustomerByEmailAsync(string email)
    {
        var customer = await customerRepository.GetCustomerByEmailAsync(email);
        return customer ?? throw new NotFoundException("Customer not found.");
    }

    public async Task<Customer> CreateCustomerAsync(CreateCustomerDto customerDto)
    {
        var existingCustomer = await customerRepository.GetCustomerByEmailAsync(customerDto.Email);
        if (existingCustomer != null)
        {
            throw new ValidationException("Customer with this email already exists.");
        }

        var customer = mapper.Map<Customer>(customerDto);
        return await customerRepository.CreateCustomerAsync(customer);
    }
}
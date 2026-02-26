using OrderManagementSystem.Domain.Entities;
using OrderManagementSystem.Domain.Interfaces;

namespace OrderManagementSystem.Application.Services;

public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public IEnumerable<Customer> GetAllCustomers()
    {
        return _customerRepository.GetAll();
    }

    public Customer? GetCustomerById(int id)
    {
        return _customerRepository.GetById(id);
    }

    public void CreateCustomer(int id, string name, string email)
    {
        var customer = new Customer(id, name, email);
        _customerRepository.Add(customer);
    }

    public void UpdateCustomer(int id, string newName, string newEmail)
    {
        var customer = _customerRepository.GetById(id);

        if (customer == null)
            throw new InvalidOperationException("Customer not found.");

        customer.UpdateName(newName);
        customer.UpdateEmail(newEmail);

        _customerRepository.Update(customer);
    }

    public void DeleteCustomer(int id)
    {
        _customerRepository.Delete(id);
    }
}
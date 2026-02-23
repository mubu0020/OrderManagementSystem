using OrderManagementSystem.Domain.Entities;

namespace OrderManagementSystem.Domain.Interfaces;

public interface IOrderRepository
{
    Order? GetById(int id);
    IEnumerable<Order> GetAll();
    void Add(Order order);
    void Update(Order order);
    void Delete(int id);
}
using OrderManagementSystem.Domain.Enums;
using System.Linq;
namespace OrderManagementSystem.Domain.Entities;

public class Order
{
    private readonly List<OrderLine> _orderLines = new();

    public int Id { get; private set; }
    public Customer Customer { get; private set; }
    public IReadOnlyCollection<OrderLine> OrderLines => _orderLines.AsReadOnly();
    public OrderStatus Status { get; private set; }

    public decimal Total => _orderLines.Sum(ol => ol.LineTotal);

    public Order(int id, Customer customer)
    {
        if (customer is null)
            throw new ArgumentNullException(nameof(customer));

        Id = id;
        Customer = customer;
        Status = OrderStatus.Pending;
    }

    public void AddOrderLine(OrderLine orderLine)
    {
        if (orderLine is null)
            throw new ArgumentNullException(nameof(orderLine));

        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Cannot modify a completed or cancelled order.");

        _orderLines.Add(orderLine);
    }

    public void RemoveOrderLine(int orderLineId)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Cannot modify a completed or cancelled order.");

        var line = _orderLines.FirstOrDefault(l => l.Id == orderLineId);

        if (line is null)
            throw new InvalidOperationException("Order line not found.");

        _orderLines.Remove(line);
    }

    public void CompleteOrder()
    {
        if (!_orderLines.Any())
            throw new InvalidOperationException("Cannot complete an empty order.");

        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Order is already processed.");

        Status = OrderStatus.Completed;
    }

    public void CancelOrder()
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Completed orders cannot be cancelled.");

        Status = OrderStatus.Cancelled;
    }
}
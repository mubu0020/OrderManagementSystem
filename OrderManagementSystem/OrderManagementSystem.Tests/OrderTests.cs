using NUnit.Framework;
using OrderManagementSystem.Domain.Entities;
using OrderManagementSystem.Domain.Enums;

namespace OrderManagementSystem.Tests;

public class OrderTests
{
    private Customer _customer = null!;
    private Product _product = null!;

    [SetUp]
    public void Setup()
    {
        _customer = new Customer(1, "Test Customer", "test@email.com");
        _product = new Product(1, "Laptop", 1000, 10);
    }

    [Test]
    public void CompleteOrder_ShouldThrow_WhenOrderIsEmpty()
    {
        var order = new Order(1, _customer);

        Assert.Throws<InvalidOperationException>(() =>
        {
            order.CompleteOrder();
        });
    }

    [Test]
    public void AddOrderLine_ShouldIncreaseTotal()
    {
        var order = new Order(1, _customer);
        var line = new OrderLine(1, _product, 2);

        order.AddOrderLine(line);

        Assert.That(order.Total, Is.EqualTo(2000));
    }

    [Test]
    public void CompleteOrder_ShouldChangeStatusToCompleted()
    {
        var order = new Order(1, _customer);
        var line = new OrderLine(1, _product, 1);

        order.AddOrderLine(line);
        order.CompleteOrder();

        Assert.That(order.Status, Is.EqualTo(OrderStatus.Completed));
    }

    [Test]
    public void AddOrderLine_ShouldThrow_WhenOrderIsCompleted()
    {
        var order = new Order(1, _customer);
        var line = new OrderLine(1, _product, 1);

        order.AddOrderLine(line);
        order.CompleteOrder();

        Assert.Throws<InvalidOperationException>(() =>
        {
            order.AddOrderLine(new OrderLine(2, _product, 1));
        });
    }
}
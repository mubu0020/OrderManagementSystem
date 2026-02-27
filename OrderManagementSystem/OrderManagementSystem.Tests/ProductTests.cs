using NUnit.Framework;
using OrderManagementSystem.Domain.Entities;

namespace OrderManagementSystem.Tests;

public class ProductTests
{
    [Test]
    public void Constructor_ShouldThrow_WhenPriceIsNegative()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var product = new Product(1, "Laptop", -100, 10);
        });
    }

    [Test]
    public void Constructor_ShouldThrow_WhenStockIsNegative()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var product = new Product(1, "Laptop", 100, -5);
        });
    }

    [Test]
    public void DecreaseStock_ShouldThrow_WhenStockBecomesNegative()
    {
        var product = new Product(1, "Laptop", 1000, 5);

        Assert.Throws<InvalidOperationException>(() =>
        {
            product.DecreaseStock(10);
        });
    }

    [Test]
    public void IncreaseStock_ShouldIncreaseStockQuantity()
    {
        var product = new Product(1, "Laptop", 1000, 5);

        product.IncreaseStock(5);

        Assert.That(product.StockQuantity, Is.EqualTo(10));
    }
}
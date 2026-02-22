namespace OrderManagementSystem.Domain.Entities;

public class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }

    public Product(int id, string name, decimal price, int stockQuantity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.");

        if (price < 0)
            throw new ArgumentException("Price cannot be negative.");

        if (stockQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative.");

        Id = id;
        Name = name;
        Price = price;
        StockQuantity = stockQuantity;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentException("Price cannot be negative.");

        Price = newPrice;
    }

    public void IncreaseStock(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Increase amount must be positive.");

        StockQuantity += amount;
    }

    public void DecreaseStock(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Decrease amount must be positive.");

        if (amount > StockQuantity)
            throw new InvalidOperationException("Not enough stock available.");

        StockQuantity -= amount;
    }
}
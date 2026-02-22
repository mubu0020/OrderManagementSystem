namespace OrderManagementSystem.Domain.Entities;

public class OrderLine
{
    public int Id { get; private set; }
    public Product Product { get; private set; }
    public int Quantity { get; private set; }

    public decimal LineTotal => Product.Price * Quantity;

    public OrderLine(int id, Product product, int quantity)
    {
        if (product is null)
            throw new ArgumentNullException(nameof(product));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        Id = id;
        Product = product;
        Quantity = quantity;
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        Quantity = newQuantity;
    }
}
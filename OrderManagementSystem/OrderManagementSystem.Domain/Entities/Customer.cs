namespace OrderManagementSystem.Domain.Entities;

public class Customer
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }

    public Customer(int id, string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.");

        Id = id;
        Name = name;
        Email = email;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.");

        Name = name;
    }

    public void UpdateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.");

        Email = email;
    }
}
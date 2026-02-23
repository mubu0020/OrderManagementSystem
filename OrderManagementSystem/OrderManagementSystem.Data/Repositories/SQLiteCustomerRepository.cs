using Microsoft.Data.Sqlite;
using OrderManagementSystem.Domain.Entities;
using OrderManagementSystem.Domain.Interfaces;

namespace OrderManagementSystem.Data.Repositories;

public class SQLiteCustomerRepository : ICustomerRepository
{
    private const string ConnectionString = "Data Source=OrderManagement.db";

    public SQLiteCustomerRepository()
    {
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        """
        CREATE TABLE IF NOT EXISTS Customers (
            Id INTEGER PRIMARY KEY,
            Name TEXT NOT NULL,
            Email TEXT NOT NULL
        );
        """;

        command.ExecuteNonQuery();
    }

    public void Add(Customer customer)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        """
        INSERT INTO Customers (Id, Name, Email)
        VALUES (@id, @name, @name);
        """;

        command.Parameters.AddWithValue("@id", customer.Id);
        command.Parameters.AddWithValue("@name", customer.Name);
        command.Parameters.AddWithValue("@email", customer.Email);

        command.ExecuteNonQuery();
    }

    public Customer? GetById(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        """
        SELECT Id, Name, Email
        FROM Customers
        WHERE Id = @id;
        """;

        command.Parameters.AddWithValue("@id", id);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Customer(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2)
            );
        }

        return null;
    }

    public IEnumerable<Customer> GetAll()
    {
        var customers = new List<Customer>();

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        """
        SELECT Id, Name, Email FROM Customers;
        """;

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            customers.Add(new Customer(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2)
            ));
        }

        return customers;
    }

    public void Update(Customer customer)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        """
        UPDATE Customers
        SET Name = @name,
            Email = @email
        WHERE Id = @id;
        """;

        command.Parameters.AddWithValue("@id", customer.Id);
        command.Parameters.AddWithValue("@name", customer.Name);
        command.Parameters.AddWithValue("@email", customer.Email);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        """
        DELETE FROM Customers
        WHERE Id = @id;
        """;

        command.Parameters.AddWithValue("@id", id);

        command.ExecuteNonQuery();
    }
}
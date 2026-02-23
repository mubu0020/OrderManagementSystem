using Microsoft.Data.Sqlite;
using OrderManagementSystem.Domain.Entities;
using OrderManagementSystem.Domain.Interfaces;

namespace OrderManagementSystem.Data.Repositories;

public class SQLiteProductRepository : IProductRepository
{
    private const string ConnectionString = "Data Source=OrderManagement.db";

    public SQLiteProductRepository()
    {
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var createCommand = connection.CreateCommand();
        createCommand.CommandText =
        """
        CREATE TABLE IF NOT EXISTS Products (
            Id INTEGER PRIMARY KEY,
            Name TEXT NOT NULL,
            Price REAL NOT NULL,
            StockQuantity INTEGER NOT NULL
        );
        """;

        createCommand.ExecuteNonQuery();

        SeedData(connection);
    }

    private void SeedData(SqliteConnection connection)
    {
        var checkCommand = connection.CreateCommand();
        checkCommand.CommandText = "SELECT COUNT(*) FROM Products;";
        var count = (long)checkCommand.ExecuteScalar();

        if (count > 0)
            return;

        var insertCommand = connection.CreateCommand();
        insertCommand.CommandText =
        """
        INSERT INTO Products (Id, Name, Price, StockQuantity) VALUES
        (1, 'Laptop', 1200.00, 10),
        (2, 'Headphones', 150.00, 25),
        (3, 'Mouse', 50.00, 40);
        """;

        insertCommand.ExecuteNonQuery();
    }

    public void Add(Product product)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        """
        INSERT INTO Products (Id, Name, Price, StockQuantity)
        VALUES (@id, @name, @price, @stock);
        """;

        command.Parameters.AddWithValue("@id", product.Id);
        command.Parameters.AddWithValue("@name", product.Name);
        command.Parameters.AddWithValue("@price", product.Price);
        command.Parameters.AddWithValue("@stock", product.StockQuantity);

        command.ExecuteNonQuery();
    }

    public Product? GetById(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        """
        SELECT Id, Name, Price, StockQuantity
        FROM Products
        WHERE Id = @id;
        """;

        command.Parameters.AddWithValue("@id", id);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Product(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetDecimal(2),
                reader.GetInt32(3)
            );
        }

        return null;
    }

    public IEnumerable<Product> GetAll()
    {
        var products = new List<Product>();

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        """
        SELECT Id, Name, Price, StockQuantity FROM Products;
        """;

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            products.Add(new Product(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetDecimal(2),
                reader.GetInt32(3)
            ));
        }

        return products;
    }

    public void Update(Product product)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        """
        UPDATE Products
        SET Name = @name,
            Price = @price,
            StockQuantity = @stock
        WHERE Id = @id;
        """;

        command.Parameters.AddWithValue("@id", product.Id);
        command.Parameters.AddWithValue("@name", product.Name);
        command.Parameters.AddWithValue("@price", product.Price);
        command.Parameters.AddWithValue("@stock", product.StockQuantity);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        """
        DELETE FROM Products
        WHERE Id = @id;
        """;

        command.Parameters.AddWithValue("@id", id);

        command.ExecuteNonQuery();
    }
}
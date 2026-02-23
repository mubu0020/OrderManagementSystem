using Microsoft.Data.Sqlite;
using OrderManagementSystem.Domain.Entities;
using OrderManagementSystem.Domain.Enums;
using OrderManagementSystem.Domain.Interfaces;

namespace OrderManagementSystem.Data.Repositories;

public class SQLiteOrderRepository : IOrderRepository
{
    private const string ConnectionString = "Data Source=OrderManagement.db";

    public SQLiteOrderRepository()
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
        CREATE TABLE IF NOT EXISTS Orders (
            Id INTEGER PRIMARY KEY,
            CustomerId INTEGER NOT NULL,
            Status INTEGER NOT NULL
        );

        CREATE TABLE IF NOT EXISTS OrderLines (
            Id INTEGER PRIMARY KEY,
            OrderId INTEGER NOT NULL,
            ProductId INTEGER NOT NULL,
            Quantity INTEGER NOT NULL
        );
        """;

        command.ExecuteNonQuery();
    }

    public void Add(Order order)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var transaction = connection.BeginTransaction();

        var orderCommand = connection.CreateCommand();
        orderCommand.Transaction = transaction;
        orderCommand.CommandText =
        """
        INSERT INTO Orders (Id, CustomerId, Status)
        VALUES (@id, @customerId, @status);
        """;

        orderCommand.Parameters.AddWithValue("@id", order.Id);
        orderCommand.Parameters.AddWithValue("@customerId", order.Customer.Id);
        orderCommand.Parameters.AddWithValue("@status", (int)order.Status);
        orderCommand.ExecuteNonQuery();

        foreach (var line in order.OrderLines)
        {
            var lineCommand = connection.CreateCommand();
            lineCommand.Transaction = transaction;
            lineCommand.CommandText =
            """
            INSERT INTO OrderLines (Id, OrderId, ProductId, Quantity)
            VALUES (@id, @orderId, @productId, @quantity);
            """;

            lineCommand.Parameters.AddWithValue("@id", line.Id);
            lineCommand.Parameters.AddWithValue("@orderId", order.Id);
            lineCommand.Parameters.AddWithValue("@productId", line.Product.Id);
            lineCommand.Parameters.AddWithValue("@quantity", line.Quantity);

            lineCommand.ExecuteNonQuery();
        }

        transaction.Commit();
    }

    public Order? GetById(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var orderCommand = connection.CreateCommand();
        orderCommand.CommandText =
        """
        SELECT Id, CustomerId, Status
        FROM Orders
        WHERE Id = @id;
        """;

        orderCommand.Parameters.AddWithValue("@id", id);

        using var reader = orderCommand.ExecuteReader();

        if (!reader.Read())
            return null;

        var orderId = reader.GetInt32(0);
        var customerId = reader.GetInt32(1);
        var status = (OrderStatus)reader.GetInt32(2);

        // NOTE: For simplicity we create minimal customer object
        var customer = new Customer(customerId, "LoadedCustomer", "loaded@email.com");

        var order = new Order(orderId, customer);

        if (status == OrderStatus.Completed)
            order.CompleteOrder();
        else if (status == OrderStatus.Cancelled)
            order.CancelOrder();

        LoadOrderLines(connection, order);

        return order;
    }

    private void LoadOrderLines(SqliteConnection connection, Order order)
    {
        var command = connection.CreateCommand();
        command.CommandText =
        """
        SELECT Id, ProductId, Quantity
        FROM OrderLines
        WHERE OrderId = @orderId;
        """;

        command.Parameters.AddWithValue("@orderId", order.Id);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var lineId = reader.GetInt32(0);
            var productId = reader.GetInt32(1);
            var quantity = reader.GetInt32(2);

            var product = new Product(productId, "LoadedProduct", 0, 0);

            var orderLine = new OrderLine(lineId, product, quantity);
            order.AddOrderLine(orderLine);
        }
    }

    public IEnumerable<Order> GetAll()
    {
        var orders = new List<Order>();

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id FROM Orders;";

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var id = reader.GetInt32(0);
            var order = GetById(id);
            if (order != null)
                orders.Add(order);
        }

        return orders;
    }

    public void Update(Order order)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        """
        UPDATE Orders
        SET Status = @status
        WHERE Id = @id;
        """;

        command.Parameters.AddWithValue("@id", order.Id);
        command.Parameters.AddWithValue("@status", (int)order.Status);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var transaction = connection.BeginTransaction();

        var deleteLines = connection.CreateCommand();
        deleteLines.Transaction = transaction;
        deleteLines.CommandText =
        """
        DELETE FROM OrderLines WHERE OrderId = @id;
        """;
        deleteLines.Parameters.AddWithValue("@id", id);
        deleteLines.ExecuteNonQuery();

        var deleteOrder = connection.CreateCommand();
        deleteOrder.Transaction = transaction;
        deleteOrder.CommandText =
        """
        DELETE FROM Orders WHERE Id = @id;
        """;
        deleteOrder.Parameters.AddWithValue("@id", id);
        deleteOrder.ExecuteNonQuery();

        transaction.Commit();
    }
}
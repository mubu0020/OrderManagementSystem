using OrderManagementSystem.Application.Services;
using OrderManagementSystem.Data.Repositories;

namespace OrderManagementSystem.ConsoleApp;

public class Program
{
    private static CustomerService _customerService = null!;
    private static ProductService _productService = null!;
    private static OrderService _orderService = null!;

    public static void Main(string[] args)
    {
        InitializeServices();
        RunMenu();
    }

    /// <summary>
    /// Manual dependency wiring.
    /// This replaces dependency injection frameworks for simplicity.
    /// </summary>
    private static void InitializeServices()
    {
        var customerRepository = new SQLiteCustomerRepository();
        var productRepository = new SQLiteProductRepository();
        var orderRepository = new SQLiteOrderRepository();

        _customerService = new CustomerService(customerRepository);
        _productService = new ProductService(productRepository);
        _orderService = new OrderService(orderRepository, customerRepository, productRepository);
    }

    private static void RunMenu()
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n=== Order Management System ===");
            Console.WriteLine("1. List Customers");
            Console.WriteLine("2. List Products");
            Console.WriteLine("3. Create Order");
            Console.WriteLine("4. List Orders");
            Console.WriteLine("0. Exit");

            Console.Write("Select option: ");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ListCustomers();
                    break;

                case "2":
                    ListProducts();
                    break;

                case "3":
                    CreateOrder();
                    break;

                case "4":
                    ListOrders();
                    break;

                case "0":
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    private static void ListCustomers()
    {
        var customers = _customerService.GetAllCustomers();

        Console.WriteLine("\n--- Customers ---");
        foreach (var customer in customers)
        {
            Console.WriteLine($"ID: {customer.Id} | {customer.Name} | {customer.Email}");
        }
    }

    private static void ListProducts()
    {
        var products = _productService.GetAllProducts();

        Console.WriteLine("\n--- Products ---");
        foreach (var product in products)
        {
            Console.WriteLine($"ID: {product.Id} | {product.Name} | Price: {product.Price} | Stock: {product.StockQuantity}");
        }
    }

    private static void CreateOrder()
    {
        Console.Write("Enter Order ID: ");
        int orderId = int.Parse(Console.ReadLine()!);

        Console.Write("Enter Customer ID: ");
        int customerId = int.Parse(Console.ReadLine()!);

        _orderService.CreateOrder(orderId, customerId);

        Console.WriteLine("Order created.");
    }

    private static void ListOrders()
    {
        var orders = _orderService.GetAllOrders();

        Console.WriteLine("\n--- Orders ---");

        foreach (var order in orders)
        {
            Console.WriteLine($"Order ID: {order.Id} | Customer: {order.Customer.Name} | Status: {order.Status} | Total: {order.Total}");

            foreach (var line in order.OrderLines)
            {
                Console.WriteLine($"   Product: {line.Product.Name} | Qty: {line.Quantity} | Line Total: {line.LineTotal}");
            }
        }
    }
}
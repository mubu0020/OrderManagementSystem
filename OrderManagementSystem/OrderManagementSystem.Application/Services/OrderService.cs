using OrderManagementSystem.Domain.Entities;
using OrderManagementSystem.Domain.Interfaces;

namespace OrderManagementSystem.Application.Services;

/* 
Service layer responsible for orchestrating order-related use cases.
This class coordinates multiple repositories and ensures that business flows
are executed correctly across aggregates.
*/
public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;

    
    /* Constructor injection of repositories.
     Dependency Inversion.
    */
    public OrderService(
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
    }

    
    // Retrieves all orders from persistence.
    public IEnumerable<Order> GetAllOrders()
    {
        return _orderRepository.GetAll();
    }

    
    /// Retrieves a single order by ID.
    public Order? GetOrderById(int id)
    {
        return _orderRepository.GetById(id);
    }

    
    /* Creates a new order for an existing customer.
     Ensures that the customer exists before creating the aggregate.*/
    
    public void CreateOrder(int orderId, int customerId)
    {
        var customer = _customerRepository.GetById(customerId);

        if (customer == null)
            throw new InvalidOperationException("Customer not found.");

        var order = new Order(orderId, customer);

        _orderRepository.Add(order);
    }

    
    /* Adds a product to an existing order.
   
   */
    public void AddProductToOrder(int orderId, int productId, int quantity, int orderLineId)
    {
        var order = _orderRepository.GetById(orderId);
        if (order == null)
            throw new InvalidOperationException("Order not found.");

        var product = _productRepository.GetById(productId);
        if (product == null)
            throw new InvalidOperationException("Product not found.");

        var orderLine = new OrderLine(orderLineId, product, quantity);

        // Aggregate root controls addition of order lines
        order.AddOrderLine(orderLine);

        _orderRepository.Update(order);
    }

    
    /* Completes an order.
     Before completion, stock levels are reduced for each product in the order.
     This ensures consistency between Order and Product aggregates.
    */
    public void CompleteOrder(int orderId)
    {
        var order = _orderRepository.GetById(orderId);
        if (order == null)
            throw new InvalidOperationException("Order not found.");

        // Reduce stock before finalizing the order
        foreach (var line in order.OrderLines)
        {
            var product = _productRepository.GetById(line.Product.Id);
            if (product == null)
                throw new InvalidOperationException("Product not found during completion.");

            product.DecreaseStock(line.Quantity);
            _productRepository.Update(product);
        }

        // Domain enforces status transition rules
        order.CompleteOrder();

        _orderRepository.Update(order);
    }

    
    /* Cancels an order.
     Status validation is handled inside the Order aggregate.
     */
    public void CancelOrder(int orderId)
    {
        var order = _orderRepository.GetById(orderId);
        if (order == null)
            throw new InvalidOperationException("Order not found.");

        order.CancelOrder();

        _orderRepository.Update(order);
    }

    
    /* Deletes an order and its associated order lines.
     Actual cascade handling is implemented in the repository layer.
    */
    public void DeleteOrder(int orderId)
    {
        _orderRepository.Delete(orderId);
    }
}
using NUnit.Framework;
using Moq;
using OrderManagementSystem.Application.Services;
using OrderManagementSystem.Domain.Interfaces;

namespace OrderManagementSystem.Tests;

public class OrderServiceTests
{
    [Test]
    public void CreateOrder_ShouldThrow_WhenCustomerDoesNotExist()
    {
        // Arrange
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockCustomerRepo = new Mock<ICustomerRepository>();
        var mockProductRepo = new Mock<IProductRepository>();

        // Simulate that customer does not exist
        mockCustomerRepo
            .Setup(repo => repo.GetById(It.IsAny<int>()))
            .Returns((Domain.Entities.Customer?)null);

        var service = new OrderService(
            mockOrderRepo.Object,
            mockCustomerRepo.Object,
            mockProductRepo.Object);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
        {
            service.CreateOrder(1, 999);
        });
    }
}
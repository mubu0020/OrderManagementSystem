using OrderManagementSystem.Domain.Entities;
using OrderManagementSystem.Domain.Interfaces;

namespace OrderManagementSystem.Application.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public IEnumerable<Product> GetAllProducts()
    {
        return _productRepository.GetAll();
    }

    public Product? GetProductById(int id)
    {
        return _productRepository.GetById(id);
    }

    public void CreateProduct(int id, string name, decimal price, int stockQuantity)
    {
        var product = new Product(id, name, price, stockQuantity);
        _productRepository.Add(product);
    }

    public void UpdateProduct(int id, string newName, decimal newPrice)
    {
        var product = _productRepository.GetById(id);

        if (product == null)
            throw new InvalidOperationException("Product not found.");

        product.UpdatePrice(newPrice);
        _productRepository.Update(product);
    }

    public void IncreaseStock(int id, int amount)
    {
        var product = _productRepository.GetById(id);

        if (product == null)
            throw new InvalidOperationException("Product not found.");

        product.IncreaseStock(amount);
        _productRepository.Update(product);
    }

    public void DecreaseStock(int id, int amount)
    {
        var product = _productRepository.GetById(id);

        if (product == null)
            throw new InvalidOperationException("Product not found.");

        product.DecreaseStock(amount);
        _productRepository.Update(product);
    }

    public void DeleteProduct(int id)
    {
        _productRepository.Delete(id);
    }
}
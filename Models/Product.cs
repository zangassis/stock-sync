using System.Text.Json;

namespace StockSync.Models;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int QuantityInStock { get; set; }
    public string Details { get; set; }
    public DateTime CreatedAt { get; private set; }


    public Product()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public Product(string name, string description, decimal price, int quantityInStock, string details) : this()
    {
        Name = name;
        Description = description;
        Price = price;
        QuantityInStock = quantityInStock;
        Details = details;
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public static Product FromJson(string json)
    {
        return JsonSerializer.Deserialize<Product>(json);
    }
}
using Microsoft.EntityFrameworkCore;
using StockSync.Models;

namespace StockSync.Data;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductHistory> ProductHistories { get; set; }
}
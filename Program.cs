using Microsoft.EntityFrameworkCore;
using StockSync.Data;
using StockSync.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProductDbContext>(x => x.UseSqlite(connectionString));

var app = builder.Build();

app.MapPost("/products/create", async (HttpContext context, ProductDbContext dbContext) =>
{
    using (var reader = new StreamReader(context.Request.Body))
    {
        string requestBody = await reader.ReadToEndAsync();

        Product deserializedProduct = Product.FromJson(requestBody);

        if (string.IsNullOrEmpty(deserializedProduct.Name))
            return Results.BadRequest("Name cannot be empty or null");

        dbContext.Products.Add(deserializedProduct);
        await dbContext.SaveChangesAsync();

        await SaveProductHistory(deserializedProduct, dbContext);

        return Results.Created($"/products/create/{deserializedProduct.Id}", deserializedProduct);
    }
});

async Task SaveProductHistory(Product product, ProductDbContext dbContext)
{
    var ProductHistory = new ProductHistory()
    {
        Id = Guid.NewGuid(),
        Product = product.ToJson()
    };

    dbContext.ProductHistories.Add(ProductHistory);
    await dbContext.SaveChangesAsync();
}

app.MapGet("/products/get-all", async (ProductDbContext dbContext) =>
{
    var products = await dbContext.Products.ToListAsync();

    return Results.Ok(products);
});

app.Run();
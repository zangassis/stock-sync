using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockSync.Data;
using StockSync.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProductDbContext>(x => x.UseSqlite(connectionString));
builder.Logging.AddJsonConsole(options =>
{
    options.IncludeScopes = false;
    options.TimestampFormat = "MM/dd/yyyy h:mm tt";
    options.JsonWriterOptions = new JsonWriterOptions
    {
        Indented = true
    };
})

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

app.MapPut("/products/update/{id}", async (ProductDbContext dbContext, Product product, Guid id) =>
{
    var existingProduct = await dbContext.Products.FindAsync(id);

    if (existingProduct == null)
        return Results.NotFound("Product not found");

    existingProduct.Name = product.Name ?? existingProduct.Name;
    existingProduct.Description = product.Description ?? existingProduct.Description;
    existingProduct.Price = product.Price != default ? product.Price : existingProduct.Price;
    existingProduct.QuantityInStock = product.QuantityInStock != default ? product.QuantityInStock : existingProduct.QuantityInStock;
    existingProduct.Details = product.Details ?? existingProduct.Details;

    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/products/delete/{id}", async (ProductDbContext dbContext, Guid id, [FromServices] ILogger<Program> logger) =>
{
    try
    {
        var existingProduct = await dbContext.Products.FindAsync(id);

        dbContext.Products.Remove(existingProduct);

        return Results.NoContent();
    }
    catch (Exception ex)
    {
        logger.LogError(ex.Message);
        return Results.NotFound("Product not found");
    }
});

app.MapPost("/products/validate", async (ProductDbContext dbContext, Product product, [FromServices] ILogger<Program> logger) =>
{
    try
    {
        var existingProduct = dbContext.Products.First(p => p.Name == product.Name);

        if (existingProduct != null)
            return Results.BadRequest("The product name must be unique");

        return Results.Ok();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"Error validating the product.");
        return Results.BadRequest(ex.Message);
    }
});

app.Run();
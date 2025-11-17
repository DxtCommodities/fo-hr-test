var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

app.MapGet("/file/file.html", async () =>
{
    return Results.Content(await File.ReadAllTextAsync("./Data/file.html"), "text/html");
});

app.MapGet("/products", async () =>
{
    return Results.File(await File.ReadAllBytesAsync("./Data/products.json"), "application/json", "products.json");
});

app.Run();

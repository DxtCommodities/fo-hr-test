using DataIntegration;
using Microsoft.Extensions.DependencyInjection;

try
{
    ServiceCollection services = new();
    services.AddSingleton(new Settings()
    {
        ProductsInfoHtmlUrl = "http://localhost:5240/file/file.html",
        ProductsJsonUrl = "http://localhost:5240/products",
        SalesCsvPath = "Data/sales.csv",
        SqlLitePath = "Data/products.db"
    });
    // TODO: register services

    var serviceProvider = services.BuildServiceProvider();

    var products = await serviceProvider.GetRequiredService<Orchestrator>().RunAsync();

    // TODO: Print summary report
    Console.WriteLine("Data integration completed successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error during integration: {ex.Message}");
}
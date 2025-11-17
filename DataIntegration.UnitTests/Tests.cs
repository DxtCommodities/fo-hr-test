using DataIntegration.Db;
using DataIntegrationExercise.Models;
using Moq;

namespace DataIntegration.lUnitTests;

public class Tests
{
    private Settings settings;

    public Tests()
    {
        this.settings = new Settings()
        {
            ProductsInfoHtmlUrl = "http://localhost:5240/file/file.html",
            ProductsJsonUrl = "http://localhost:5240/products",
            SalesCsvPath = "Data/sales.csv",
            SqlLitePath = "Data/products.db"
        };
    }

    [Test]
    public async Task BaseTest()
    {
        // Prodotti mock
        var products = new[] {
            new Product { Id = 1, Name = "Laptop Pro 15", Category = "Computers", Price = 100m, Country="USA",
                Sales = [
                    new Sale { DateUtc = new DateTime(2025, 11, 01, 11, 21, 00, DateTimeKind.Utc),  Quantity = 2 },
                    new Sale { DateUtc = new DateTime(2025, 11, 14, 11, 21, 00, DateTimeKind.Utc), Quantity = 3 },
                ],
                OnSaleFromMonth = new DateOnly(2025, 11, 1),
                WithdrawnFromSaleInMonth = new DateOnly(2025, 12, 1) // one month on sale
            },
            new Product { Id = 2, Name = "Wireless Mouse", Category = "Accessories", Price = 50m, Country="USA",
                Sales = [
                    new Sale { DateUtc = new DateTime(2025, 11, 13, 11, 21, 00, DateTimeKind.Utc),  Quantity = 2 },
                    new Sale { DateUtc = new DateTime(2025, 12, 14, 11, 21, 00, DateTimeKind.Utc), Quantity = 3 },
                ],
                OnSaleFromMonth = new DateOnly(2025, 11, 1),
                WithdrawnFromSaleInMonth = new DateOnly(2026, 1, 1) // two months on sale
            },
            new Product { Id = 3, Name = "Coke", Category = "Drink&Food", Price = 3.00m, Country="ITA",
                Sales = [
                    new Sale { DateUtc = new DateTime(2025, 12, 01, 11, 21, 00, DateTimeKind.Utc),  Quantity = 2 },
                    new Sale { DateUtc = new DateTime(2025, 12, 14, 11, 21, 00, DateTimeKind.Utc), Quantity = 3 },
                ],
                OnSaleFromMonth = new DateOnly(2025, 11, 1),
                WithdrawnFromSaleInMonth = new DateOnly(2025, 12, 1) // one month on sale
            }
        };

        var repository = new Repository(this.settings);

        var result = repository.GetAggregations(products, new DateOnly(2025, 11, 1), new DateOnly(2026, 01, 01));

        await Assert.That(result.Count).IsEqualTo(3); // ita novembre, usa novembre, usa dicembre
        await Assert.That(result.Count(r => r.Country == "USA")).IsEqualTo(2);
        await Assert.That(result.Count(r => r.Country != "USA")).IsEqualTo(1);

        // both USA products sold both months
        await Assert.That(result.FirstOrDefault(r => r.Country == "USA" && r.Month == new DateOnly(2025, 11, 1))?.TotalNumberDistinctOfProducts ?? 0).IsEqualTo(2);
        await Assert.That(result.FirstOrDefault(r => r.Country == "USA" && r.Month == new DateOnly(2025, 12, 1))?.TotalNumberDistinctOfProducts ?? 0).IsEqualTo(1);

        await Assert.That(result.FirstOrDefault(r => r.Country == "USA" && r.Month == new DateOnly(2025, 11, 1))?.TotalRevenew ?? 0).IsEqualTo(600);
        await Assert.That(result.FirstOrDefault(r => r.Country == "USA" && r.Month == new DateOnly(2025, 12, 1))?.TotalRevenew ?? 0).IsEqualTo(150);
    }

    [Test]
    public async Task ProductSaleValidity()
    {
        // Prodotti mock
        var products = new[] {
            new Product { Id = 1, Name = "Laptop Pro 15", Category = "Computers", Price = 100m, Country="USA",
                Sales = [
                    new Sale { DateUtc = new DateTime(2025, 11, 01, 11, 21, 00, DateTimeKind.Utc),  Quantity = 2 },
                    new Sale { DateUtc = new DateTime(2025, 11, 14, 11, 21, 00, DateTimeKind.Utc), Quantity = 3 },
                ],
                OnSaleFromMonth = new DateOnly(2025, 11, 1),
                WithdrawnFromSaleInMonth = new DateOnly(2025, 12, 1)
            },
            new Product { Id = 2, Name = "Wireless Mouse", Category = "Accessories", Price = 50m, Country="USA",
                Sales = [
                    new Sale { DateUtc = new DateTime(2025, 11, 13, 11, 21, 00, DateTimeKind.Utc),  Quantity = 2 },
                    new Sale { DateUtc = new DateTime(2025, 12, 14, 11, 21, 00, DateTimeKind.Utc), Quantity = 3 }, // sell over validity
                ],
                OnSaleFromMonth = new DateOnly(2025, 11, 1),
                WithdrawnFromSaleInMonth = new DateOnly(2025, 12, 1)
            },
            new Product { Id = 3, Name = "Coke", Category = "Drink&Food", Price = 3.00m, Country="ITA",
                Sales = [
                    new Sale { DateUtc = new DateTime(2025, 12, 01, 11, 21, 00, DateTimeKind.Utc),  Quantity = 2 },
                    new Sale { DateUtc = new DateTime(2025, 12, 14, 11, 21, 00, DateTimeKind.Utc), Quantity = 3 },
                ],
                OnSaleFromMonth = new DateOnly(2025, 11, 1),
                WithdrawnFromSaleInMonth = new DateOnly(2025, 12, 1)
            }
        };

        var repository = new Repository(this.settings);

        var result = repository.GetAggregations(products, new DateOnly(2025, 11, 1), new DateOnly(2026, 01, 01));

        await Assert.That(result.Count).IsEqualTo(2); // ita novembre, usa novembre
        await Assert.That(result.Count(r => r.Country == "USA")).IsEqualTo(1);
        await Assert.That(result.Count(r => r.Country != "USA")).IsEqualTo(1);

        // one products on sales here
        await Assert.That(result.FirstOrDefault(r => r.Country == "USA" && r.Month == new DateOnly(2025, 11, 1))?.TotalNumberDistinctOfProducts ?? 0).IsEqualTo(2);
        await Assert.That(result.FirstOrDefault(r => r.Country == "USA" && r.Month == new DateOnly(2025, 12, 1))?.TotalNumberDistinctOfProducts ?? 0).IsEqualTo(0);


        await Assert.That(result.FirstOrDefault(r => r.Country == "USA" && r.Month == new DateOnly(2025, 11, 1))?.TotalRevenew ?? 0).IsEqualTo(600);
        await Assert.That(result.FirstOrDefault(r => r.Country == "USA" && r.Month == new DateOnly(2025, 12, 1))?.TotalRevenew ?? 0).IsEqualTo(0);
    }


    [Test]
    public async Task NoSalesProduct()
    {
        // Prodotti mock
        var products = new[] {
            new Product { Id = 1, Name = "Laptop Pro 15", Category = "Computers", Price = 100m, Country="USA",
                Sales = [
                    new Sale { DateUtc = new DateTime(2025, 11, 01, 11, 21, 00, DateTimeKind.Utc),  Quantity = 2 },
                    new Sale { DateUtc = new DateTime(2025, 11, 14, 11, 21, 00, DateTimeKind.Utc), Quantity = 3 },
                ],
                OnSaleFromMonth = new DateOnly(2025, 11, 1),
                WithdrawnFromSaleInMonth = new DateOnly(2025, 12, 1)},
            new Product { Id = 2, Name = "Wireless Mouse", Category = "Accessories", Price = 50m, Country="USA",
                Sales = [
                ],
                OnSaleFromMonth = new DateOnly(2025, 11, 1),
                WithdrawnFromSaleInMonth = new DateOnly(2026, 01, 1)},
            new Product { Id = 3, Name = "Coke", Category = "Drink&Food", Price = 3.00m, Country="ITA",
                Sales = [
                    new Sale { DateUtc = new DateTime(2025, 12, 01, 11, 21, 00, DateTimeKind.Utc),  Quantity = 2 },
                ],
                OnSaleFromMonth = new DateOnly(2025, 11, 1),
                WithdrawnFromSaleInMonth = new DateOnly(2025, 12, 1)
            }
        };

        var repository = new Repository(this.settings);

        var result = repository.GetAggregations(products, new DateOnly(2025, 11, 1), new DateOnly(2026, 01, 01));

        await Assert.That(result.Count).IsEqualTo(3); // ita novembre, usa novembre, usa december
        await Assert.That(result.Count(r => r.Country == "USA")).IsEqualTo(2);
        await Assert.That(result.Count(r => r.Country != "USA")).IsEqualTo(1);
        await Assert.That(result.FirstOrDefault(r => r.Country == "USA" && r.Month == new DateOnly(2025, 11, 1))?.TotalNumberDistinctOfProducts ?? 0).IsEqualTo(2);
        await Assert.That(result.FirstOrDefault(r => r.Country == "USA" && r.Month == new DateOnly(2025, 12, 1))?.TotalNumberDistinctOfProducts ?? 0).IsEqualTo(1);


        await Assert.That(result.FirstOrDefault(r => r.Country == "USA" && r.Month == new DateOnly(2025, 11, 1))?.AveragePriceOfProducts ?? 0).IsEqualTo(75m);
        await Assert.That(result.FirstOrDefault(r => r.Country == "USA" && r.Month == new DateOnly(2025, 12, 1))?.AveragePriceOfProducts ?? 0).IsEqualTo(50m);

        await Assert.That(result.FirstOrDefault(r => r.Country == "USA" && r.Month == new DateOnly(2025, 11, 1))?.TotalRevenew ?? 0).IsEqualTo(500);
        await Assert.That(result.FirstOrDefault(r => r.Country == "USA" && r.Month == new DateOnly(2025, 12, 1))?.TotalRevenew ?? 0).IsEqualTo(0);
    }

    [Test]
    public async Task GroupByCet_Bonus()
    {
        // Prodotti mock
        var products = new[] {
            new Product { Id = 1, Name = "Laptop Pro 15", Category = "Computers", Price = 100m, Country="USA",
                Sales = [
                    new Sale { DateUtc = new DateTime(2025, 11, 30, 23, 50, 00, DateTimeKind.Utc),  Quantity = 2 }, // december CET!
                    new Sale { DateUtc = new DateTime(2025, 12, 01, 00, 00, 00, DateTimeKind.Utc), Quantity = 3 }, // december CET!
                    new Sale { DateUtc = new DateTime(2025, 11, 01, 00, 00, 00, DateTimeKind.Utc), Quantity = 5 }, // novembre CET!
                ],
                OnSaleFromMonth = new DateOnly(2025, 11, 1),
                WithdrawnFromSaleInMonth = new DateOnly(2026, 01, 01)
            },
            new Product { Id = 2, Name = "Wireless Mouse", Category = "Accessories", Price = 50m, Country="USA",
                Sales = [
                    new Sale { DateUtc = new DateTime(2025, 11, 01, 23, 40, 00, DateTimeKind.Utc),  Quantity = 1 }, // december CET => out of product validity range
                ],
                OnSaleFromMonth = new DateOnly(2025, 11, 1),
                WithdrawnFromSaleInMonth = new DateOnly(2025, 12, 01)
            },
            new Product { Id = 3, Name = "Coke", Category = "Drink&Food", Price = 3.00m, Country="ITA",
                Sales = [
                    new Sale { DateUtc = new DateTime(2025, 12, 01, 11, 21, 00, DateTimeKind.Utc),  Quantity = 2 },
                ],
                OnSaleFromMonth = new DateOnly(2025, 11, 1),
                WithdrawnFromSaleInMonth = new DateOnly(2025, 12, 1)
            }
        };

        var repository = new Repository(this.settings);

        var result = repository.GetAggregations(products, new DateOnly(2025, 11, 1), new DateOnly(2026, 01, 01));

        await Assert.That(result.Count).IsEqualTo(3); // ita novembre, usa novembre, usa december
        await Assert.That(result.Count(r => r.Country == "USA")).IsEqualTo(2);
        await Assert.That(result.Count(r => r.Country != "USA")).IsEqualTo(1);
        await Assert.That(result.FirstOrDefault(r => r.Country == "USA" && r.Month == new DateOnly(2025, 11, 1))?.TotalNumberDistinctOfProducts ?? 0).IsEqualTo(2);
        await Assert.That(result.FirstOrDefault(r => r.Country == "USA" && r.Month == new DateOnly(2025, 11, 1))?.AveragePriceOfProducts ?? 0).IsEqualTo(75);
        await Assert.That(result.FirstOrDefault(r => r.Country == "USA" && r.Month == new DateOnly(2025, 12, 1))?.TotalNumberDistinctOfProducts ?? 0).IsEqualTo(1);
        await Assert.That(result.FirstOrDefault(r => r.Country == "USA" && r.Month == new DateOnly(2025, 12, 1))?.AveragePriceOfProducts ?? 0).IsEqualTo(100);

        await Assert.That(result.FirstOrDefault(r => r.Country == "USA" && r.Month == new DateOnly(2025, 12, 1))?.TotalRevenew ?? 0).IsEqualTo(200+300);
        await Assert.That(result.FirstOrDefault(r => r.Country == "USA" && r.Month == new DateOnly(2025, 11, 1))?.TotalRevenew ?? 0).IsEqualTo(550);
    }
}